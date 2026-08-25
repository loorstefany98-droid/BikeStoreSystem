
using BikeStore.Datos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BikeStore.Datos.Models;

namespace BikeStore.API.Controllers
{
    [Route("api/bicicletas")]
    [ApiController]
    public class BicicletasController : ControllerBase
    {
        private readonly string _cadenaConexion;

        public BicicletasController(IConfiguration configuration)
        {
            _cadenaConexion = configuration.GetConnectionString("CadenaSQL");
        }

        // GET: api/bicicletas
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var db = new BicicletaRepository(_cadenaConexion);
                var lista = db.ObtenerBicicletas();

                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new { mensaje = "Error al obtener las bicicletas: " + ex.Message }
                );
            }
        }

        // GET: api/bicicletas/{id}
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var db = new BicicletaRepository(_cadenaConexion);

                var bicicleta = db.ObtenerPorId(id);

                if (bicicleta == null)
                    return NotFound(new
                    {
                        mensaje = "Bicicleta no encontrada"
                    });

                return Ok(bicicleta);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new { mensaje = "Error al obtener la bicicleta: " + ex.Message }
                );
            }
        }

        // GET: api/bicicletas/stock-critico
        [HttpGet("stock-critico")]
        public IActionResult GetStockCritico()
        {
            try
            {
                var db = new BicicletaRepository(_cadenaConexion);
                var lista = db.ObtenerStockCritico();

                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new { mensaje = "Error al obtener el stock crítico: " + ex.Message }
                );
            }
        }

        // POST: api/bicicletas
        [HttpPost]
        public IActionResult Post([FromBody] Bicicleta bicicleta)
        {
            try
            {
                if (bicicleta.Stock == 0)
                    bicicleta.Estado = "Agotado";
                else if (bicicleta.Stock >= 1 && bicicleta.Stock <= 15)
                    bicicleta.Estado = "Stock bajo";
                else
                    bicicleta.Estado = "Disponible";

                var db = new BicicletaRepository(_cadenaConexion);
                bool respuesta = db.Registrar(bicicleta);

                if (!respuesta)
                    return BadRequest(new
                    {
                        mensaje = "Error: La categoría no existe o no se pudo registrar."
                    });

                return StatusCode(
                    StatusCodes.Status201Created,
                    new { mensaje = "Bicicleta registrada con éxito" }
                );
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new { mensaje = "Error al registrar la bicicleta: " + ex.Message }
                );
            }
        }

        // PUT: api/bicicletas/{id}
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Bicicleta bicicleta)
        {
            try
            {
                bicicleta.IdBicicleta = id;

                if (bicicleta.Stock == 0)
                    bicicleta.Estado = "Agotado";
                else if (bicicleta.Stock >= 1 && bicicleta.Stock <= 15)
                    bicicleta.Estado = "Stock bajo";
                else
                    bicicleta.Estado = "Disponible";

                var db = new BicicletaRepository(_cadenaConexion);
                bool respuesta = db.Actualizar(bicicleta);

                if (!respuesta)
                    return NotFound(new
                    {
                        mensaje = "No se pudo actualizar: La bicicleta no existe"
                    });

                return Ok(new
                {
                    mensaje = "Bicicleta actualizada con éxito"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new { mensaje = "Error al actualizar la bicicleta: " + ex.Message }
                );
            }
        }

        // DELETE: api/bicicletas/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var db = new BicicletaRepository(_cadenaConexion);
                bool respuesta = db.Eliminar(id);

                if (!respuesta)
                    return NotFound(new
                    {
                        mensaje = "No se pudo eliminar: La bicicleta no existe"
                    });

                return Ok(new
                {
                    mensaje = "Bicicleta eliminada con éxito"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new { mensaje = "Error al eliminar la bicicleta: " + ex.Message }
                );
            }
        }
    }
}

