using Newtonsoft.Json;
using SistemaGestion.Dtos;
using SistemaGestion.Interfaces;

namespace SistemaGestion.Servicios
{
    public class ServiceContacto : IContacto 
    {
        private readonly ISqlQueryDynamicJson _sql;

        public ServiceContacto(ISqlQueryDynamicJson sql)
        {
            _sql = sql;
        }

        public string InsertarContacto(string json)
        {
            RespuestaSpDto respuestaDto = new RespuestaSpDto();
            string respuesta;
            try
            {
                var ContactoDto = JsonConvert.DeserializeObject<ContactoDto>(json);
                if (string.IsNullOrEmpty(ContactoDto.Nombre) ||
                    string.IsNullOrEmpty(ContactoDto.Email) ||
                    string.IsNullOrEmpty(ContactoDto.Telefono) ||
                    string.IsNullOrEmpty(ContactoDto.Mensaje)
                   )

                {
                    respuestaDto.idrespuesta = 0;
                    respuestaDto.mensaje = new { codigo = "Datos Faltantes" };
                    return JsonConvert.SerializeObject(respuestaDto);
                }

                respuesta = _sql.EjecutarQuery(json, "gen.spInsertarContacto");

                if (string.IsNullOrEmpty(respuesta))
                {
                    respuestaDto.idrespuesta = 0;
                    respuestaDto.mensaje = new { codigo = "Error BaseDatos" };
                    return JsonConvert.SerializeObject(respuestaDto);
                }
            }
            catch (Exception ex)
            {
                respuestaDto.idrespuesta = 0;
                respuestaDto.mensaje = new { codigo = "Error Interno", detalle = ex.Message };
                respuesta = JsonConvert.SerializeObject(respuestaDto);
            }

            return respuesta;
        }
    }
}
