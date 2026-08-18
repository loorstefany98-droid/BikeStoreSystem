using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BikeStore.Web.Models;

namespace BikeStore.Web.Services
{
    public class CategoriaApiService
    {
        private readonly HttpClient _httpClient;

        public CategoriaApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Categoria>> ObtenerTodasAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Categoria>>("api/categorias")
                   ?? new List<Categoria>();
        }
    }
}
