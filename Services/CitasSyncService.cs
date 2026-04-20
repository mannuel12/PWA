using PWA.Models;

namespace PWA.Services;

public class CitasSyncService
{
    private const string StoreName = "citas";

    private readonly IndexedDbService _indexedDbService;
    private readonly CitasApiService _citasApiService;

    public CitasSyncService(IndexedDbService indexedDbService, CitasApiService citasApiService)
    {
        _indexedDbService = indexedDbService;
        _citasApiService = citasApiService;
    }

    public async Task<List<CitaMedica>> ObtenerTodasAsync()
    {
        var citas = await _indexedDbService.GetAllAsync<CitaMedica>(StoreName);

        return citas
            .Where(c => !c.IsDeleted)
            .OrderBy(c => c.FechaHora)
            .ToList();
    }

    public async Task AgregarLocalAsync(CitaMedica cita)
    {
        cita.LocalId ??= Guid.NewGuid().ToString();
        cita.LastModifiedUtc = DateTime.UtcNow;
        cita.SyncStatus = "Pending";

        await _indexedDbService.PutAsync(StoreName, cita);
    }

    public async Task EliminarLocalAsync(string localId)
    {
        var citas = await _indexedDbService.GetAllAsync<CitaMedica>(StoreName);
        var cita = citas.FirstOrDefault(c => c.LocalId == localId);

        if (cita is null)
        {
            return;
        }

        cita.IsDeleted = true;
        cita.SyncStatus = "Pending";
        cita.LastModifiedUtc = DateTime.UtcNow;

        await _indexedDbService.PutAsync(StoreName, cita);
    }

    public async Task<int> SincronizarAsync()
    {
        var procesadas = 0;

        var citas = await _indexedDbService.GetAllAsync<CitaMedica>(StoreName);
        var pendientes = citas
            .Where(c => c.SyncStatus == "Pending" && !c.IsDeleted)
            .OrderBy(c => c.LastModifiedUtc)
            .ToList();

        foreach (var cita in pendientes)
        {
            var serverId = await _citasApiService.CrearEnServidorAsync(cita);

            if (serverId.HasValue)
            {
                cita.ServerId = serverId.Value;
                cita.SyncStatus = "Synced";
                cita.LastModifiedUtc = DateTime.UtcNow;

                await _indexedDbService.PutAsync(StoreName, cita);
                procesadas++;
            }
        }

        return procesadas;
    }
}