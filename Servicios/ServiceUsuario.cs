
using Newtonsoft.Json;
using SistemaGestion.Dtos;
using SistemaGestion.Helpers;
using SistemaGestion.Interfaces;

namespace SistemaGestion.Servicios
{
    public class ServiceUsuario : IUsuario
    {
        private readonly ISqlQueryDynamicJson _sql;

        public ServiceUsuario(ISqlQueryDynamicJson sql)
        {
            _sql = sql;
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

                respuesta = _sql.EjecutarQuery(json, "seg.spIniciarSesion");

                if (string.IsNullOrEmpty(respuesta))
                {
                    respuestaDto.idrespuesta = 0;
                    respuestaDto.mensaje = new { codigo = "CredencialesInvalidas" };
                    respuesta = JsonConvert.SerializeObject(respuestaDto);
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
    }
}


