using System.Net.Http.Json;
using PWA.Models;

namespace PWA.Services;

public class TratamientosApiService
{
    private readonly HttpClient _httpClient;

    public TratamientosApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<int?> CrearEnServidorAsync(Tratamiento tratamiento)
    {
        var payload = new
        {
            id = 0,
            nombre = tratamiento.Nombre,
            dosis = tratamiento.Dosis,
            frecuencia = tratamiento.Frecuencia,
            fechaInicio = tratamiento.FechaInicio,
            fechaFin = tratamiento.FechaFin,
            activo = tratamiento.Activo,
            notas = tratamiento.Notas,
            lastModifiedUtc = DateTime.UtcNow
        };

        var response = await _httpClient.PostAsJsonAsync("https://localhost:7110/api/Tratamientos", payload);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var creado = await response.Content.ReadFromJsonAsync<TratamientoServidorResponse>();
        return creado?.Id;
    }

    private class TratamientoServidorResponse
    {
        public int Id { get; set; }
    }
}