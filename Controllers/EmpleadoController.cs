using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SistemaGestion.Dtos;
using SistemaGestion.Interfaces;

namespace SistemaGestion.Controllers
{
    [Route("api/empleado")]
    [ApiController]
    [Authorize]

    public class EmpleadoController : ControllerBase
    {
        private readonly IEmpleado _empleado;

        public EmpleadoController(IEmpleado empleado)
        {
            _empleado = empleado;
        }

        [HttpGet("cargar/{EmpleadoID}", Name = "CargarEmpleado")]
        public IActionResult CargarEmpleado(int EmpleadoID)
        {
            try
            {
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized("Token no proporcionado o inválido.");
                }

                var respuesta = _empleado.CargarEmpleado(EmpleadoID);

                if (string.IsNullOrEmpty(respuesta))
                {
                    return NotFound(new { mensaje = "Empleado no encontrado" });
                }
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = "Error al ejecutar", detalle = ex.Message });
            }
        }

        [HttpPost("agregar", Name = "AgregarEmpleado")]
        public IActionResult AgregarEmpleado([FromBody] EmpleadoDto empleado)
        {
            try
            {
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized("Token no proporcionado o inválido.");
                }

                var json = JsonConvert.SerializeObject(empleado);
                var respuesta = _empleado.AgregarEmpleado(json);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = "Error al ejecutar", detalle = ex.Message });
            }
        }

        [HttpGet("consultar/{EmpleadoID}", Name = "ConsultarEmpleado")]
        public IActionResult ConsultarEmpleado(int EmpleadoID)
        {
            try
            {
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized("Token no proporcionado o inválido.");
                }

                var respuesta = _empleado.ConsultarEmpleado(EmpleadoID);
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


        [HttpPut("actualizar", Name = "ActualizarEmpleado")]
        public IActionResult ActualizarEmpleado([FromBody] EmpleadoDto empleado)
        {
            try
            {
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized("Token no proporcionado o inválido.");
                }

                var json = JsonConvert.SerializeObject(empleado);
                var respuesta = _empleado.ActualizarEmpleado(json);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = "Error al ejecutar", detalle = ex.Message });
            }
        }

        [HttpDelete("eliminar/{EmpleadoID}", Name = "EliminarEmpleado")]
        public IActionResult EliminarEmpleado(int EmpleadoID)
        {
            try
            {
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized("Token no proporcionado o inválido.");
                }

                var respuesta = _empleado.EliminarEmpleado(EmpleadoID);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = "Error al ejecutar", detalle = ex.Message });
            }
        }

    }
}
