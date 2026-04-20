using System.Text.Json;
using PWA.Models;

namespace PWA.Services;

/* Servicio encargado de administrar y persistir tratamientos en localStorage.
public class TratamientosService
{
    private readonly LocalStorageService _localStorageService;
    private const string Key = "tratamientos_usuario";

    private List<Tratamiento> _tratamientos = new();

    public TratamientosService(LocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }

    // Carga los tratamientos guardados desde localStorage.
    public async Task CargarAsync()
    {
        var data = await _localStorageService.ObtenerAsync(Key);

        if (!string.IsNullOrWhiteSpace(data))
        {
            _tratamientos = JsonSerializer.Deserialize<List<Tratamiento>>(data) ?? new List<Tratamiento>();
        }
    }

    // Devuelve todos los tratamientos, activos primero y luego por fecha de inicio más reciente.
    public List<Tratamiento> ObtenerTodos()
    {
        return _tratamientos
            .OrderByDescending(t => t.Activo)
            .ThenByDescending(t => t.FechaInicio)
            .ToList();
    }

    // Agrega un tratamiento y guarda cambios.
    public async Task AgregarAsync(Tratamiento tratamiento)
    {
        _tratamientos.Add(tratamiento);
        await GuardarCambiosAsync();
    }

    // Elimina un tratamiento por Id y guarda cambios.
    public async Task EliminarAsync(Guid id)
    {
        var tratamiento = _tratamientos.FirstOrDefault(t => t.Id == id);

        if (tratamiento is not null)
        {
            _tratamientos.Remove(tratamiento);
            await GuardarCambiosAsync();
        }
    }

    // Cambia el estado activo/finalizado de un tratamiento.
    public async Task CambiarEstadoAsync(Guid id)
    {
        var tratamiento = _tratamientos.FirstOrDefault(t => t.Id == id);

        if (tratamiento is not null)
        {
            tratamiento.Activo = !tratamiento.Activo;
            await GuardarCambiosAsync();
        }
    }

    // Guarda la lista serializada en localStorage.
    private async Task GuardarCambiosAsync()
    {
        var json = JsonSerializer.Serialize(_tratamientos);
        await _localStorageService.GuardarAsync(Key, json);
    }
}*/