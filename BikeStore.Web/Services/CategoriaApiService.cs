using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using BikeStore.Web.Models;

namespace BikeStore.Web.Services
{
    public class CategoriaApiService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public CategoriaApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Categoria>> ObtenerTodasAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Categoria>>("api/categorias")
                   ?? new List<Categoria>();
        }

        public async Task<Categoria?> ObtenerPorIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/categorias/{id}");
            if (!response.IsSuccessStatusCode) return null;
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Categoria>(json, _jsonOptions);
        }

        public async Task<(bool Exito, string? Error)> RegistrarAsync(Categoria categoria)
        {
            var content = new StringContent(
                JsonSerializer.Serialize(categoria), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/categorias", content);

            if (response.IsSuccessStatusCode)
                return (true, null);

            var error = await response.Content.ReadAsStringAsync();
            return (false, error);
        }

        public async Task<(bool Exito, string? Error)> ActualizarAsync(int id, Categoria categoria)
        {
            var content = new StringContent(
                JsonSerializer.Serialize(categoria), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"api/categorias/{id}", content);

            if (response.IsSuccessStatusCode)
                return (true, null);

            var error = await response.Content.ReadAsStringAsync();
            return (false, error);
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/categorias/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
