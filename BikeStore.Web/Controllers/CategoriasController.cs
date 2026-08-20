using Microsoft.AspNetCore.Mvc;
using BikeStore.Web.Services;
using BikeStore.Web.Models;

namespace BikeStore.Web.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly CategoriaApiService _apiService;

        public CategoriasController(CategoriaApiService apiService)
        {
            _apiService = apiService;
        }

        // GET: /Categorias
        public async Task<IActionResult> Index()
        {
            var categorias = await _apiService.ObtenerTodasAsync();
            return View(categorias);
        }

        // GET: /Categorias/Crear
        public IActionResult Crear()
        {
            return View();
        }

        // POST: /Categorias/Crear
        [HttpPost]
        public async Task<IActionResult> Crear(Categoria categoria)
        {
            if (!ModelState.IsValid)
                return View(categoria);

            var (exito, error) = await _apiService.RegistrarAsync(categoria);

            if (!exito)
            {
                ModelState.AddModelError("", error ?? "No se pudo registrar la categoría.");
                return View(categoria);
            }

            TempData["Mensaje"] = "Categoría registrada con éxito.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Categorias/Editar/5
        public async Task<IActionResult> Editar(int id)
        {
            var categoria = await _apiService.ObtenerPorIdAsync(id);
            if (categoria == null)
                return NotFound();

            return View(categoria);
        }

        // POST: /Categorias/Editar/5
        [HttpPost]
        public async Task<IActionResult> Editar(int id, Categoria categoria)
        {
            if (!ModelState.IsValid)
                return View(categoria);

            var (exito, error) = await _apiService.ActualizarAsync(id, categoria);

            if (!exito)
            {
                ModelState.AddModelError("", error ?? "No se pudo actualizar la categoría.");
                return View(categoria);
            }

            TempData["Mensaje"] = "Categoría actualizada con éxito.";
            return RedirectToAction(nameof(Index));
        }

        // POST: /Categorias/Eliminar/5
        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            await _apiService.EliminarAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
