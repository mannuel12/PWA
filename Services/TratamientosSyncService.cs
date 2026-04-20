using PWA.Models;

namespace PWA.Services;

public class TratamientosSyncService
{
    private const string StoreName = "tratamientos";

    private readonly IndexedDbService _indexedDbService;
    private readonly TratamientosApiService _tratamientosApiService;

    public TratamientosSyncService(
        IndexedDbService indexedDbService,
        TratamientosApiService tratamientosApiService)
    {
        _indexedDbService = indexedDbService;
        _tratamientosApiService = tratamientosApiService;
    }

    public async Task<List<Tratamiento>> ObtenerTodosAsync()
    {
        var tratamientos = await _indexedDbService.GetAllAsync<Tratamiento>(StoreName);

        return tratamientos
            .Where(t => !t.IsDeleted)
            .OrderByDescending(t => t.Activo)
            .ThenByDescending(t => t.FechaInicio)
            .ToList();
    }

    public async Task AgregarLocalAsync(Tratamiento tratamiento)
    {
        tratamiento.LocalId ??= Guid.NewGuid().ToString();
        tratamiento.LastModifiedUtc = DateTime.UtcNow;
        tratamiento.SyncStatus = "Pending";

        await _indexedDbService.PutAsync(StoreName, tratamiento);
    }

    public async Task EliminarLocalAsync(string localId)
    {
        var tratamientos = await _indexedDbService.GetAllAsync<Tratamiento>(StoreName);
        var tratamiento = tratamientos.FirstOrDefault(t => t.LocalId == localId);

        if (tratamiento is null)
        {
            return;
        }

        tratamiento.IsDeleted = true;
        tratamiento.SyncStatus = "Pending";
        tratamiento.LastModifiedUtc = DateTime.UtcNow;

        await _indexedDbService.PutAsync(StoreName, tratamiento);
    }

    public async Task CambiarEstadoLocalAsync(string localId)
    {
        var tratamientos = await _indexedDbService.GetAllAsync<Tratamiento>(StoreName);
        var tratamiento = tratamientos.FirstOrDefault(t => t.LocalId == localId);

        if (tratamiento is null)
        {
            return;
        }

        tratamiento.Activo = !tratamiento.Activo;
        tratamiento.SyncStatus = "Pending";
        tratamiento.LastModifiedUtc = DateTime.UtcNow;

        await _indexedDbService.PutAsync(StoreName, tratamiento);
    }

    public async Task<int> SincronizarAsync()
    {
        var procesados = 0;

        var tratamientos = await _indexedDbService.GetAllAsync<Tratamiento>(StoreName);
        var pendientes = tratamientos
            .Where(t => t.SyncStatus == "Pending" && !t.IsDeleted)
            .OrderBy(t => t.LastModifiedUtc)
            .ToList();

        foreach (var tratamiento in pendientes)
        {
            var serverId = await _tratamientosApiService.CrearEnServidorAsync(tratamiento);

            if (serverId.HasValue)
            {
                tratamiento.ServerId = serverId.Value;
                tratamiento.SyncStatus = "Synced";
                tratamiento.LastModifiedUtc = DateTime.UtcNow;

                await _indexedDbService.PutAsync(StoreName, tratamiento);
                procesados++;
            }
        }

        return procesados;
    }
}