using Newtonsoft.Json;
using SistemaGestion.Dtos;
using SistemaGestion.Interfaces;
using SistemaGestion.Servicios;
using System.Data;
using System.Threading.Tasks;
using static SistemaGestion.Dtos.GeografiaDto;

namespace SistemaGestion.Servicios
{
    public class ServiceGeografia : IGeografia
    {
        private readonly ISqlQueryDynamicJson _sql;
        public ServiceGeografia(ISqlQueryDynamicJson sql)
        {
            _sql = sql;
        }
        public string ObtenerEstructuraGeografica()
        {
            RespuestaSpDto respuestaDto = new RespuestaSpDto();
            string respuesta;

            try
            {
                // Ejecutar el procedimiento almacenado y obtener la respuesta
                // string jsonRespuesta = _sql.EjecutarQuery("{}", "gen.spObtenerEstructuraGeografica");
                respuesta = _sql.EjecutarQuery("{}", "gen.spObtenerEstructuraGeografica");

                if (string.IsNullOrEmpty(respuesta))
                {
                    respuestaDto.idrespuesta = 0;
                    respuestaDto.mensaje = new { codigo = "Error al ejecutar el procedimiento almacenado" };
                    return JsonConvert.SerializeObject(respuestaDto);
                }

                // Deserializar el JSON recibido en la estructura RespuestaSpDto
                respuestaDto = JsonConvert.DeserializeObject<RespuestaSpDto>(respuesta);

                // Validar que la respuesta contenga datos
                if (respuestaDto == null || respuestaDto.geograficas == null || !respuestaDto.geograficas.Any())
                {
                    respuestaDto = new RespuestaSpDto
                    {
                        idrespuesta = 0,
                        mensaje = new { codigo = "No se encontraron datos" },
                        geograficas = new List<GeografiaDto.PaisDto>()
                    };
                }
            }
            catch (Exception ex)
            {
                respuestaDto.idrespuesta = 0;
                respuestaDto.mensaje = new { codigo = "Error interno", detalle = ex.Message };
            }

            // Serializar y devolver la respuesta
            return JsonConvert.SerializeObject(respuestaDto);
        }
    }
  
}


