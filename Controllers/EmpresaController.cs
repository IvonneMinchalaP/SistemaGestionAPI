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
        [HttpGet("obtener-empresas", Name = "ObtenerEmpresas")]
        [Authorize]
        public IActionResult ObtenerEmpresas()
        {
            try
            {
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { mensaje = "Token no proporcionado" });
                }

                var respuesta = _empresa.ObtenerEmpresas();
                if (string.IsNullOrEmpty(respuesta))
                {
                    return NotFound(new { mensaje = "No se encontraron empresas" });
                }

                return Ok(JsonSerializer.Deserialize<object>(respuesta));
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = "Error al ejecutar", detalle = ex.Message });
            }
        }

        [HttpPost("agregar", Name = "AgregarEmpresa")]
        [Authorize]

        public IActionResult AgregarEmpresa([FromBody] EmpresaDto empresa)
        {
            try
            {
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized("Token no proporcionado o inválido.");
                }

                var json = JsonSerializer.Serialize(empresa);
                var respuesta = _empresa.AgregarEmpresa(json);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = "Error al ejecutar", detalle = ex.Message });
            }
        }


        [HttpPut("actualizar", Name = "ActualizarEmpresa")]
        public IActionResult ActualizarEmpresa([FromBody] EmpresaDto empresa)
        {
            try
            {
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized("Token no proporcionado o inválido.");
                }

                var json = JsonSerializer.Serialize(empresa);
                var respuesta = _empresa.ActualizarEmpresa(json);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = "Error al ejecutar", detalle = ex.Message });
            }
        }

        [HttpGet("cargar", Name = "CargarEmpresa")]
        [Authorize]
        public IActionResult CargarEmpresa([FromQuery] int empresaID)
        {
            try
            {
                //var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (empresaID <= 0)
                {
                    return BadRequest(new { mensaje = "El ID de la empresa es inválido" });
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




