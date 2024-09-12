using MagicVillaAPI.Data;
using MagicVillaAPI.Models;
using MagicVillaAPI.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVillaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        // clase para mensages por consola
        private readonly ILogger<VillaController> _logger;
        private readonly AplicacionDBContext _dBContext;
        public VillaController(ILogger<VillaController> logger, AplicacionDBContext dBContext)
        {
            this._logger = logger;
            this._dBContext = dBContext;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<VillaDto>> GetVillas()
        {
            _logger.LogInformation("Obtener villas");
            //return Ok(VillaStore.VillaList);
            return Ok(_dBContext.Villas.ToList());
        }

        [HttpGet("id:int", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDto> GetVilla(int id)
        {
            if (id == 0)
            {

                _logger.LogError("Error al traer la villa con ID " + id);
                return BadRequest();
            }
            //  var villa = VillaStore.VillaList.FirstOrDefault(v => v.Id == id);
            var villa = _dBContext.Villas.FirstOrDefault(v => v.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            _logger.LogInformation("Obtener una villa");
            return Ok(villa);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDto> CrearVilla(
            [FromBody] VillaDto villa)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            if (villa == null)
            {
                return BadRequest(villa);
            }
            //VillaStore.VillaList.FirstOrDefault(v => v.Nombre.ToLower() == villa.Nombre.ToLower()) != null
            if (_dBContext.Villas.FirstOrDefault(v => v.Nombre.ToLower() == villa.Nombre.ToLower()) != null) {
                // agrega mensajes personalizados al ModalState
                ModelState.AddModelError("NombreExiste", "Ya existe la villa que quiere ingresar");
                return BadRequest(ModelState);
            }

            if (villa.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            // agrega valores a listado 
            //   villa.Id = VillaStore.VillaList.OrderByDescending(v => v.Id).FirstOrDefault().Id + 1;
            // VillaStore.VillaList.Add(villa);
            // funciona ok y cuando se requere retornar el nuevo valor

            Villa nueva = new Villa() { 
                Id = villa.Id,
                Nombre = villa.Nombre,
                Detalle = villa.Detalle,
                ImagenURL= villa.ImagenURL,
                Tarifa= villa.Tarifa,
                FechaActualizacion=DateTime.Now,
                FechaCreacion=DateTime.Now
            };
            _dBContext.Villas.Add(nueva);
            _dBContext.SaveChanges();
            _logger.LogInformation("Agregar una villa");
            return CreatedAtRoute("GetVilla", new { id = villa.Id }, villa);

        }


        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
          //  var villa = VillaStore.VillaList.FirstOrDefault(v => v.Id == id);
            var villa = _dBContext.Villas.FirstOrDefault(v => v.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            //VillaStore.VillaList.Remove(villa);
            _dBContext.Villas.Remove(villa);
            _dBContext.SaveChanges();
            // se devuelve para verificar que se borro
            return NoContent();
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateVilla(int id, [FromBody] VillaDto villa)
        {
            if (id == 0 || villa == null)
            {
                return BadRequest();
            }
            // var villaBuscada = VillaStore.VillaList.FirstOrDefault(v => v.Id == id);
            var villaBuscada = _dBContext.Villas.FirstOrDefault(v => v.Id == id);
            if (villaBuscada == null)
            {
                return NotFound();
            }
            villaBuscada.Nombre = villa.Nombre;
            villaBuscada.ImagenURL= villa.ImagenURL;
            villaBuscada.Tarifa= villa.Tarifa;
            villaBuscada.Detalle= villa.Detalle;
            villaBuscada.FechaActualizacion = DateTime.Now;

            _dBContext.SaveChanges();
            // se devuelve para verificar que se borro
            return NoContent();
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateParcialVilla(int id, JsonPatchDocument<VillaDto> villa)
        {
            if (id == 0 || villa == null)
            {
                return BadRequest();
            }
            //var villaBuscada = VillaStore.VillaList.FirstOrDefault(v => v.Id == id);
            var villaBuscada = _dBContext.Villas.AsNoTracking().FirstOrDefault(v => v.Id == id);
            if (villaBuscada == null)
            {
                return NotFound();
            }

            VillaDto actualizacion = new VillaDto() { 
                Id = villaBuscada.Id,
                Nombre = villaBuscada.Nombre,
                Detalle= villaBuscada.Detalle,
                ImagenURL= villaBuscada.ImagenURL,
                Tarifa= villaBuscada.Tarifa,

            };

            villa.ApplyTo(actualizacion, ModelState);

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            Villa actualizada = new Villa() { 
                Id= actualizacion.Id,
            
                Nombre = actualizacion.Nombre,
                Detalle = actualizacion.Detalle,
                ImagenURL = actualizacion.ImagenURL,
                Tarifa = actualizacion.Tarifa,
                FechaActualizacion = DateTime.Now
            };
            _dBContext.Villas.Update(actualizada);
            _dBContext.SaveChanges();
            // se devuelve para verificar que se borro
            return NoContent();
        }

    }
}
