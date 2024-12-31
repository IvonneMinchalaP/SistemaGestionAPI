﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using SistemaGestion.Interfaces;
using SistemaGestion.Dtos;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;


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

        [HttpGet("obtener-empleado", Name = "ObtenerEmpleados")]
        [Authorize]
        public IActionResult ObtenerEmpleado()
        {
            try
            {
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { mensaje = "Token no proporcionado" });
                }

                var respuesta = _empleado.ObtenerEmpleado();
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

                var jsonRequest = JsonSerializer.Serialize(empleado);
                var respuesta = _empleado.AgregarEmpleado(jsonRequest);
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

                var jsonRequest = JsonSerializer.Serialize(empleado);
                var respuesta = _empleado.ActualizarEmpleado(jsonRequest);
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
