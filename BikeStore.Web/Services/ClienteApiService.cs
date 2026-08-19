using System.Text;
using System.Text.Json;
using BikeStore.Web.Models;

namespace BikeStore.Web.Services
{
    public class ClienteApiService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public ClienteApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Cliente>> ObtenerTodosAsync()
        {
            var response = await _httpClient.GetAsync("api/clientes");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Cliente>>(json, _jsonOptions) ?? new List<Cliente>();
        }

        public async Task<Cliente?> ObtenerPorIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/clientes/{id}");
            if (!response.IsSuccessStatusCode) return null;
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Cliente>(json, _jsonOptions);
        }

        public async Task<(bool Exito, string? Error)> RegistrarAsync(Cliente cliente)
        {
            var content = new StringContent(
                JsonSerializer.Serialize(cliente), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/clientes", content);

            if (response.IsSuccessStatusCode)
                return (true, null);

            var error = await response.Content.ReadAsStringAsync();
            return (false, error);
        }

        public async Task<(bool Exito, string? Error)> ActualizarAsync(int id, Cliente cliente)
        {
            var content = new StringContent(
                JsonSerializer.Serialize(cliente), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"api/clientes/{id}", content);

            if (response.IsSuccessStatusCode)
                return (true, null);

            var error = await response.Content.ReadAsStringAsync();
            return (false, error);
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/clientes/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
