using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using SistemaGestion.Interfaces;
using SistemaGestion.Dtos;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Security.Claims;
using DocumentFormat.OpenXml.Wordprocessing;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace SistemaGestion.Controllers
{
    [ApiController]
    [Route("api/usuario")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuario _usuario;
        private readonly IConfiguration _configuration;


        public UsuarioController(IUsuario usuario, IConfiguration configuration)
        {
            _usuario = usuario;
            _configuration = configuration;

        }

        // Método para iniciar sesión 
        [HttpPost("iniciar-sesion", Name = "IniciarSesion")]
        public IActionResult IniciarSesion([FromBody] UsuarioDto usuarioDto)
        {
            try
            {
                var jsonSerializer = JsonSerializer.Serialize(usuarioDto);
                var respuesta = _usuario.IniciarSesion(jsonSerializer);
                return Ok(JsonSerializer.Deserialize<object>(respuesta));

            }
            catch (System.Exception ex)
            {
                return BadRequest(new { mensaje = "Error al iniciar sesión", detalle = ex.Message });
            }
        }

        // Método para registrar un usuario
        [HttpPost("registrar", Name = "RegistrarUsuario")]
        public IActionResult RegistrarUsuario([FromBody] JsonElement json)
        {
            try
            {
                // Convertir el JSON recibido a un string para pasarlo al servicio
                var jsonSerializer = JsonSerializer.Serialize(json);
                var respuesta = _usuario.RegistrarUsuario(jsonSerializer);

                // Retornar la respuesta desde el servicio
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = "Error al ejecutar", detalle = ex.Message });
            }
        }

        // Método para consultar usuario (requiere autorización)
        [HttpGet("consultar", Name = "ConsultarUsuario")]
        [Authorize]
        public IActionResult ConsultarUsuario([FromQuery] int usuarioID)
        {
            try
            {
                if (usuarioID <= 0)
                {
                    return BadRequest(new { mensaje = "UsuarioID inválido" });
                }

                var jsonRequest = JsonSerializer.Serialize(new { UsuarioID = usuarioID });

                var respuesta = _usuario.ConsultarUsuario(jsonRequest);

                if (string.IsNullOrEmpty(respuesta))
                {
                    return NotFound(new { mensaje = "Usuario no encontrado" });
                }

                return Ok(JsonSerializer.Deserialize<object>(respuesta));
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = "Error al ejecutar", detalle = ex.Message });
            }
        }

        // Método para actualizar usuario (requiere autorización)
        [HttpPut("actualizar", Name = "ActualizarUsuario")]
        [Authorize]
        public IActionResult ActualizarUsuario([FromBody] JsonElement json)
        {
            try
            {
                //Extraemos el token de la cabecera para la autorización
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized("Token no proporcionado o inválido.");
                }

                // Convertir el JSON recibido a un string para pasarlo al servicio
                var jsonSerializer = JsonSerializer.Serialize(json);
                var respuesta = _usuario.ActualizarUsuario(jsonSerializer);

                // Retornar la respuesta desde el servicio
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = "Error al ejecutar", detalle = ex.Message });
            }

        }

      
      
    }
}


