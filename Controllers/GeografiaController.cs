using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaGestion.Interfaces;
using System.Text.Json;
using System.Threading.Tasks;

namespace SistemaGestion.Controllers
{
    [Route("api/geografia")]
    [ApiController]
    public class GeografiaController : ControllerBase
    {
        private readonly IGeografia _geografia;

        public GeografiaController(IGeografia geografia)
        {
            _geografia = geografia;
        }

        [HttpGet("obtener-estructura-geografica", Name = "ObtenerEstructuraGeografica")]
        [Authorize]
        public IActionResult ObtenerEstructuraGeografica()

        {
            try
            {
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { mensaje = "Token no proporcionado" });
                }
                var respuesta =  _geografia.ObtenerEstructuraGeografica();
                if (string.IsNullOrEmpty(respuesta))
                {
                    return NotFound(new { mensaje = "No se encontraron empresas" });
                }
                var data = JsonSerializer.Deserialize<object>(respuesta);
                return Ok(data);
                //return Ok(JsonSerializer.Deserialize<object>(respuesta));
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = "Error al ejecutar", detalle = ex.Message });
            }
        }
    }
}
