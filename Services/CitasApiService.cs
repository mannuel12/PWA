using System.Net.Http.Json;
using PWA.Models;

namespace PWA.Services;

public class CitasApiService
{
    private readonly HttpClient _httpClient;

    public CitasApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<int?> CrearEnServidorAsync(CitaMedica cita)
    {
        var payload = new
        {
            id = 0,
            lugar = cita.Lugar,
            tipoConsulta = cita.TipoConsulta,
            fechaHora = cita.FechaHora,
            notas = cita.Notas,
            lastModifiedUtc = DateTime.UtcNow
        };

        var response = await _httpClient.PostAsJsonAsync("https://localhost:7110/api/Citas", payload);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var creada = await response.Content.ReadFromJsonAsync<CitaServidorResponse>();
        return creada?.Id;
    }

    private class CitaServidorResponse
    {
        public int Id { get; set; }
    }
}