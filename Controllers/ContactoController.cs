using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SistemaGestion.Dtos;
using SistemaGestion.Interfaces;

namespace SistemaGestion.Controllers
{

    [Route("api/contacto")]
    [ApiController]
    public class ContactoController : ControllerBase
    {
        private readonly IContacto _contacto;

        public ContactoController(IContacto contacto)
        {
            _contacto = contacto;
        }

        [HttpPost("insertar", Name = "InsertarContacto")]
        [Authorize]
        public IActionResult InsertarContacto([FromBody] ContactoDto contacto)
        {
            try
            {
                var json = JsonConvert.SerializeObject(contacto);
                var respuesta = _contacto.InsertarContacto(json);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = "Error al ejecutar", detalle = ex.Message });
            }
        }
    }
}
