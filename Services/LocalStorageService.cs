using Microsoft.JSInterop;

namespace PWA.Services;

// Servicio encargado de comunicarse con localStorage del navegador
// mediante JavaScript Interop.
public class LocalStorageService
{
    private readonly IJSRuntime _jsRuntime;

    public LocalStorageService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    // Guarda un string en localStorage con una clave dada.
    public async Task GuardarAsync(string key, string value)
    {
        await _jsRuntime.InvokeVoidAsync("storageHelper.save", key, value);
    }

    // Obtiene un string desde localStorage usando su clave.
    public async Task<string?> ObtenerAsync(string key)
    {
        return await _jsRuntime.InvokeAsync<string?>("storageHelper.get", key);
    }

    // Elimina un registro de localStorage.
    public async Task EliminarAsync(string key)
    {
        await _jsRuntime.InvokeVoidAsync("storageHelper.remove", key);
    }
}