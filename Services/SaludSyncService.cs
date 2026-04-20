using PWA.Models;

namespace PWA.Services;

public class SaludSyncService
{
    private const string StoreName = "salud";

    private readonly IndexedDbService _indexedDbService;
    private readonly SaludApiService _saludApiService;

    public SaludSyncService(IndexedDbService indexedDbService, SaludApiService saludApiService)
    {
        _indexedDbService = indexedDbService;
        _saludApiService = saludApiService;
    }

    public async Task<List<RegistroSalud>> ObtenerTodosAsync()
    {
        var registros = await _indexedDbService.GetAllAsync<RegistroSalud>(StoreName);

        return registros
            .Where(r => !r.IsDeleted)
            .OrderByDescending(r => r.FechaRegistro)
            .ToList();
    }

    public async Task AgregarLocalAsync(RegistroSalud registro)
    {
        registro.LocalId ??= Guid.NewGuid().ToString();
        registro.LastModifiedUtc = DateTime.UtcNow;
        registro.SyncStatus = "Pending";

        await _indexedDbService.PutAsync(StoreName, registro);
    }

    public async Task EliminarLocalAsync(string localId)
    {
        var registros = await _indexedDbService.GetAllAsync<RegistroSalud>(StoreName);
        var registro = registros.FirstOrDefault(r => r.LocalId == localId);

        if (registro is null)
        {
            return;
        }

        registro.IsDeleted = true;
        registro.SyncStatus = "Pending";
        registro.LastModifiedUtc = DateTime.UtcNow;

        await _indexedDbService.PutAsync(StoreName, registro);
    }

    public async Task<int> SincronizarAsync()
    {
        var procesados = 0;

        var registros = await _indexedDbService.GetAllAsync<RegistroSalud>(StoreName);
        var pendientes = registros
            .Where(r => r.SyncStatus == "Pending" && !r.IsDeleted)
            .OrderBy(r => r.LastModifiedUtc)
            .ToList();

        foreach (var registro in pendientes)
        {
            var serverId = await _saludApiService.CrearEnServidorAsync(registro);

            if (serverId.HasValue)
            {
                registro.ServerId = serverId.Value;
                registro.SyncStatus = "Synced";
                registro.LastModifiedUtc = DateTime.UtcNow;

                await _indexedDbService.PutAsync(StoreName, registro);
                procesados++;
            }
        }

        return procesados;
    }
}