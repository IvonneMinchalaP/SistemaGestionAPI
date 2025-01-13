
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SistemaGestion.Dtos;
using SistemaGestion.Helpers;
using SistemaGestion.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SistemaGestion.Servicios
{
    public class ServiceUsuario : IUsuario
    {
        private readonly ISqlQueryDynamicJson _sql;
        private readonly IConfiguration _configuration;

        public ServiceUsuario(ISqlQueryDynamicJson sql, IConfiguration configuration)
        {
            _sql = sql;
            _configuration = configuration;

        }
        public string IniciarSesion(string json)
        {
            RespuestaSpDto respuestaDto = new RespuestaSpDto();
            string respuesta;

            try
            {
                var usuarioDto = JsonConvert.DeserializeObject<UsuarioDto>(json);
                if (string.IsNullOrEmpty(usuarioDto.Email) || string.IsNullOrEmpty(usuarioDto.Contrasena))
                {
                    respuestaDto.idrespuesta = 0;
                    respuestaDto.mensaje = new { codigo = "CredencialesFaltantes" };
                    return JsonConvert.SerializeObject(respuestaDto);
                }

                // Llamar al procedimiento almacenado para iniciar sesión
                respuesta = _sql.EjecutarQuery(json, "seg.spIniciarSesion");

                // Validar la respuesta del procedimiento
                if (string.IsNullOrEmpty(respuesta))
                {
                    respuestaDto.idrespuesta = 0;
                    respuestaDto.mensaje = new { codigo = "ErrorBaseDatos" };
                    return JsonConvert.SerializeObject(respuestaDto);
                }

                // Parsear la respuesta del procedimiento almacenado
                var resultadoSp = JsonConvert.DeserializeObject<Dictionary<string, string>>(respuesta);

                if (resultadoSp.ContainsKey("Mensaje") && resultadoSp["Mensaje"] == "Inicio de sesión exitoso")
                {
                    // Generar el token JWT
                    var token = GenerarToken(new UsuarioDto
                    {
                        UsuarioID = int.Parse(resultadoSp["UsuarioID"]),
                        Nombre = resultadoSp["Nombre"],
                        Email = resultadoSp["Email"]
                    });

                    respuestaDto.idrespuesta = 1;
                    respuestaDto.mensaje = new
                    {
                        codigo = "InicioSesionExitoso",
                        token,
                        usuarioID = resultadoSp["UsuarioID"],
                        nombre = resultadoSp["Nombre"]
                    };
                }
                else
                {
                    respuestaDto.idrespuesta = 0;
                    respuestaDto.mensaje = new { codigo = resultadoSp["Mensaje"] };
                }

                return JsonConvert.SerializeObject(respuestaDto);
            }
            catch (Exception ex)
            {
                respuestaDto.idrespuesta = 0;
                respuestaDto.mensaje = new { codigo = "ErrorInterno", detalle = ex.Message };
                respuesta = JsonConvert.SerializeObject(respuestaDto);
            }

            return respuesta;
        }

        // Método para generar el token JWT
            private string GenerarToken(UsuarioDto usuario)
        {
            var key = Encoding.UTF8.GetBytes(_configuration["AppSettings:SecretKey"]);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("UsuarioID", usuario.UsuarioID.ToString()),
                new Claim("Nombre", usuario.Nombre)
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

        public string RegistrarUsuario(string json)
        {
            RespuestaSpDto respuestaDto = new RespuestaSpDto();
            string respuesta;
            try
            {
                var usuarioDto = JsonConvert.DeserializeObject<UsuarioDto>(json);
                if (string.IsNullOrEmpty(usuarioDto.Nombre) ||
                    string.IsNullOrEmpty(usuarioDto.Apellido) ||
                    string.IsNullOrEmpty(usuarioDto.Email) ||
                    string.IsNullOrEmpty(usuarioDto.Contrasena) ||
                    string.IsNullOrEmpty(usuarioDto.Genero) ||
                    string.IsNullOrEmpty(usuarioDto.FechaNacimiento))

                {
                    respuestaDto.idrespuesta = 0;
                    respuestaDto.mensaje = new { codigo = "DatosFaltantes" };
                    return JsonConvert.SerializeObject(respuestaDto);
                }

                respuesta = _sql.EjecutarQuery(json, "seg.spInsertarUsuario");

                if (string.IsNullOrEmpty(respuesta))
                {
                    respuestaDto.idrespuesta = 0;
                    respuestaDto.mensaje = new { codigo = "ErrorBaseDatos" };
                    return JsonConvert.SerializeObject(respuestaDto);
                }
            }
            catch (Exception ex)
            {
                respuestaDto.idrespuesta = 0;
                respuestaDto.mensaje = new { codigo = "ErrorInterno", detalle = ex.Message };
                respuesta = JsonConvert.SerializeObject(respuestaDto);
            }

            return respuesta;
        }
        public string ConsultarUsuario(string json)
        {
            RespuestaSpDto respuestaDto = new RespuestaSpDto();
            string respuesta;
            try
            {
                var usuarioDto = JsonConvert.DeserializeObject<UsuarioDto>(json);
                if (usuarioDto.UsuarioID == null)

                {
                    respuestaDto.idrespuesta = 0;
                    respuestaDto.mensaje = new { codigo = "Usuario No Encontrado" };
                    return JsonConvert.SerializeObject(respuestaDto);
                }

                respuesta = _sql.EjecutarQuery(json, "seg.spConsultarUsuario");

               
            }
            catch (Exception ex)
            {
                respuestaDto.idrespuesta = 0;
                respuestaDto.mensaje = new { codigo = "ErrorInterno", detalle = ex.Message };
                respuesta = JsonConvert.SerializeObject(respuestaDto);
            }

            return respuesta;
        }

        public string ActualizarUsuario(string json)
        {
            RespuestaSpDto respuestaDto = new RespuestaSpDto();
            string respuesta;
            try
            {
                var usuarioDto = JsonConvert.DeserializeObject<UsuarioDto>(json);
                if (usuarioDto.UsuarioID == null ||
                    string.IsNullOrEmpty(usuarioDto.Nombre) ||
                    string.IsNullOrEmpty(usuarioDto.Apellido) ||
                    string.IsNullOrEmpty(usuarioDto.Email) ||
                    string.IsNullOrEmpty(usuarioDto.Genero) ||
                    string.IsNullOrEmpty(usuarioDto.FechaNacimiento))

                {
                    respuestaDto.idrespuesta = 0;
                    respuestaDto.mensaje = new { codigo = "DatosFaltantes" };
                    return JsonConvert.SerializeObject(respuestaDto);
                }

                respuesta = _sql.EjecutarQuery(json, "seg.spActualizarUsuario");

                if (string.IsNullOrEmpty(respuesta))
                {
                    respuestaDto.idrespuesta = 0;
                    respuestaDto.mensaje = new { codigo = "ErrorBaseDatos" };
                    return JsonConvert.SerializeObject(respuestaDto);
                }
            }
            catch (Exception ex)
            {
                respuestaDto.idrespuesta = 0;
                respuestaDto.mensaje = new { codigo = "ErrorInterno", detalle = ex.Message };
                respuesta = JsonConvert.SerializeObject(respuestaDto);
            }

            return respuesta;
        }

    }
}


