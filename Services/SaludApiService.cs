using System.Net.Http.Json;
using PWA.Models;

namespace PWA.Services;

public class SaludApiService
{
    private readonly HttpClient _httpClient;

    public SaludApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<int?> CrearEnServidorAsync(RegistroSalud registro)
    {
        var payload = new
        {
            id = 0,
            fechaRegistro = registro.FechaRegistro,
            peso = registro.Peso,
            presionSistolica = registro.PresionSistolica,
            presionDiastolica = registro.PresionDiastolica,
            notas = registro.Notas,
            lastModifiedUtc = DateTime.UtcNow
        };

        var response = await _httpClient.PostAsJsonAsync("https://localhost:7110/api/Salud", payload);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var creado = await response.Content.ReadFromJsonAsync<RegistroServidorResponse>();
        return creado?.Id;
    }

    private class RegistroServidorResponse
    {
        public int Id { get; set; }
    }
}