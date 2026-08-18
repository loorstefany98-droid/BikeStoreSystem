using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BikeStore.Web.Services;
using BikeStore.Web.Models;
using System.Threading.Tasks;

namespace BikeStore.Web.Controllers
{
    public partial class BicicletasController : Controller
    {
        private readonly BicicletaApiService _apiService;
        private readonly CategoriaApiService _catService;

        public BicicletasController(BicicletaApiService apiService, CategoriaApiService catService)
        {
            _apiService = apiService;
            _catService = catService;
        }

        // --- Método auxiliar para cargar las categorías en el ViewBag ---
        private async Task CargarCategoriasEnViewBag(int? categoriaSeleccionada = null)
        {
            var categorias = await _catService.ObtenerTodasAsync();
            ViewBag.ListaCategorias = new SelectList(categorias, "IdCategoria", "Nombre", categoriaSeleccionada);
        }

        // --- GET: Bicicletas ---
        public async Task<IActionResult> Index(string? marca, string? categoria)
        {
            var bicicletas = (marca != null || categoria != null)
                ? await _apiService.BuscarAsync(marca, categoria)
                : await _apiService.ObtenerTodasAsync();

            ViewBag.MarcaFiltro = marca;
            ViewBag.CategoriaFiltro = categoria;

            await CargarCategoriasEnViewBag(string.IsNullOrEmpty(categoria) ? null : int.Parse(categoria));

            return View(bicicletas);
        }

        // --- GET: Bicicletas/Crear ---
        public async Task<IActionResult> Crear()
        {
            await CargarCategoriasEnViewBag();
            return View();
        }

        // --- POST: Bicicletas/Crear ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Bicicleta bicicleta)
        {
            if (ModelState.IsValid)
            {
                var creado = await _apiService.RegistrarAsync(bicicleta);
                if (creado)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            // Si falla, volvemos a cargar las categorías para no perder el desplegable en la vista
            await CargarCategoriasEnViewBag(bicicleta.IdCategoria);
            ModelState.AddModelError(string.Empty, "No se pudo registrar la bicicleta.");
            return View(bicicleta);
        }

        // --- GET: Bicicletas/Editar/5 ---
        public async Task<IActionResult> Editar(int id)
        {
            var bicicleta = await _apiService.ObtenerPorIdAsync(id);
            if (bicicleta == null)
            {
                return NotFound();
            }

            await CargarCategoriasEnViewBag(bicicleta.IdCategoria);
            return View(bicicleta);
        }

        // --- POST: Bicicletas/Editar/5 ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, Bicicleta bicicleta)
        {
            if (id != bicicleta.IdBicicleta)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var actualizado = await _apiService.ActualizarAsync(id, bicicleta);
                if (actualizado)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            await CargarCategoriasEnViewBag(bicicleta.IdCategoria);
            ModelState.AddModelError(string.Empty, "No se pudo actualizar la bicicleta.");
            return View(bicicleta);
        }

        // --- GET: Bicicletas/Eliminar/5 ---
        public async Task<IActionResult> Eliminar(int id)
        {
            var bicicleta = await _apiService.ObtenerPorIdAsync(id);
            if (bicicleta == null)
            {
                return NotFound();
            }
            return View(bicicleta);
        }

        // --- POST: Bicicletas/Eliminar/5 ---
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            var eliminado = await _apiService.EliminarAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}