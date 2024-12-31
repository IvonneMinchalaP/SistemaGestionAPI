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

        // Método para iniciar sesión 
        [HttpPost("iniciar-sesion", Name = "IniciarSesion")]
        public async Task<IActionResult> IniciarSesion([FromBody] UsuarioDto usuarioDto)
        {
            try
            {
                // Verificar si el usuario existe y la contraseña es correcta
                var usuarioValido = await ValidarUsuario(usuarioDto.Email, usuarioDto.Contrasena);

                if (usuarioValido != null)
                {
                    // Generar el token JWT
                    var token = GenerarToken(usuarioValido);

                    // Incluir el UsuarioID en la respuesta
                    return Ok(new { token, usuarioID = usuarioValido.UsuarioID });
                }
                else
                {
                    return Unauthorized("Usuario o contraseña incorrectos");
                }
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { mensaje = "Error al iniciar sesión", detalle = ex.Message });
            }
        }

        // Método para validar al usuario en la base de datos
        private async Task<UsuarioDto> ValidarUsuario(string email, string contrasena)
        {
            // Verificar si el usuario existe en la base de datos
            using (var sql = new SqlConnection(_configuration.GetConnectionString("Development")))
            {
                var query = "SELECT UsuarioID, Nombre, Apellido, Email FROM gen.tbUsuarios WHERE Email = @Email AND Contrasena = @Contrasena";
                var command = new SqlCommand(query, sql);

                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Contrasena", contrasena);

                await sql.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        // Retorna el usuario si se encuentra en la base de datos
                        return new UsuarioDto
                        {
                            UsuarioID = reader.GetInt32(0),
                            Email = reader.GetString(1)
                        };
                    }
                }
            }
            return null;
        }

        // Método para generar el token JWT
        private string GenerarToken(UsuarioDto usuario)
        {
            var key = Encoding.UTF8.GetBytes(_configuration["AppSettings:SecretKey"]);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(2), // El token expirará en 2 horas
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
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

      
    }
}


