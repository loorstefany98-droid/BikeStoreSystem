using System.Text;
using System.Text.Json;
using BikeStore.Web.Models;

namespace BikeStore.Web.Services
{
    public class VentaApiService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public VentaApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Venta>> ObtenerTodasAsync()
        {
            var response = await _httpClient.GetAsync("api/ventas");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Venta>>(json, _jsonOptions) ?? new List<Venta>();
        }

        public async Task<Venta?> ObtenerPorIdAsync(int id)
        {
            var todas = await ObtenerTodasAsync();
            return todas.FirstOrDefault(v => v.IdVenta == id);
        }

        public async Task<(bool Exito, string? Error, JsonElement? Resultado)> RegistrarAsync(RegistrarVentaRequest venta)
        {
            var content = new StringContent(
                JsonSerializer.Serialize(venta), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/ventas", content);

            var body = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var resultado = JsonSerializer.Deserialize<JsonElement>(body, _jsonOptions);
                return (true, null, resultado);
            }

            return (false, body, null);
        }
    }
}
