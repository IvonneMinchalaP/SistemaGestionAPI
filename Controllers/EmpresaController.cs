using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using SistemaGestion.Interfaces;
using SistemaGestion.Dtos;

namespace SistemaGestion.Controllers
{
    [ApiController]
    [Route("api/empresa")]
    [Authorize]
    public class EmpresaController : ControllerBase
    {
        private readonly IEmpresa _empresa;
        private readonly IConfiguration _configuration;


        public EmpresaController(IEmpresa empresa, IConfiguration configuration)
        {
            _empresa = empresa;
            _configuration = configuration;

        }

        [HttpGet("cargar", Name = "CargarEmpresa")]
        public IActionResult CargarEmpresa([FromQuery] int empresaID)
        {
            try
            {
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized("Token no proporcionado o inválido.");
                }

                var jsonRequest = JsonSerializer.Serialize(new { EmpresaID = empresaID });
                var respuesta = _empresa.CargarEmpresa(jsonRequest);

                if (string.IsNullOrEmpty(respuesta))
                {
                    return NotFound(new { mensaje = "Empresa no encontrado" });
                }

                return Ok(JsonSerializer.Deserialize<object>(respuesta));
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = "Error al ejecutar", detalle = ex.Message });
            }
        }

        [HttpPost("agregar", Name = "AgregarEmpresa")]
        public IActionResult AgregarEmpresa([FromBody] EmpresaDto empresa)
        {
            try
            {
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized("Token no proporcionado o inválido.");
                }

                var json = JsonConvert.SerializeObject(empresa);
                var respuesta = _empresa.AgregarEmpresa(json);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = "Error al ejecutar", detalle = ex.Message });
            }
        }

        [HttpGet("consultar/{EmpresaID}", Name = "ConsultarEmpresa")]
        public IActionResult ConsultarEmpresa(int EmpresaID)
        {
            try
            {
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized("Token no proporcionado o inválido.");
                }

                var respuesta = _empresa.ConsultarEmpresa(EmpresaID);
                if (string.IsNullOrEmpty(respuesta))
                {
                    return NotFound(new { mensaje = "Empresa no encontrado" });
                }
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = "Error al ejecutar", detalle = ex.Message });
            }
        }
       

        [HttpPut ("actualizar", Name = "ActualizarEmpresa")]
        public IActionResult ActualizarEmpresa([FromBody] EmpresaDto empresa)
        {
            try
            {
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized("Token no proporcionado o inválido.");
                }

                var json = JsonConvert.SerializeObject(empresa);
                var respuesta = _empresa.ActualizarEmpresa(json);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = "Error al ejecutar", detalle = ex.Message });
            }
        }

        [HttpDelete("eliminar/{EmpresaID}", Name = "EliminarEmpresa")]
        public IActionResult EliminarEmpresa(int EmpresaID)
        {
            try
            {
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized("Token no proporcionado o inválido.");
                }

                var respuesta = _empresa.EliminarEmpresa(EmpresaID);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = "Error al ejecutar", detalle = ex.Message });
            }
        }
    }
}




