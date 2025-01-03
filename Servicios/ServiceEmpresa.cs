using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using SistemaGestion.Dtos;
using SistemaGestion.Interfaces;
using System.Data;

namespace SistemaGestion.Servicios
{
    public class ServiceEmpresa : IEmpresa
    {
        private readonly ISqlQueryDynamicJson _sql;

        public ServiceEmpresa(ISqlQueryDynamicJson sql)
        {
            _sql = sql;
        }
        public string ObtenerEmpresas()
        {
            RespuestaSpDto respuestaDto = new RespuestaSpDto();
            string respuesta;

            try
            {
                respuesta = _sql.EjecutarQuery("{}", "gen.spObtenerEmpresas");
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

        public string AgregarEmpresa(string json)
        {
            RespuestaSpDto respuestaDto = new RespuestaSpDto();
            string respuesta;
            try
            {
                var EmpresaDto = JsonConvert.DeserializeObject<EmpresaDto>(json);
                if (string.IsNullOrEmpty(EmpresaDto.Nombre) ||
                    string.IsNullOrEmpty(EmpresaDto.Email) ||
                    string.IsNullOrEmpty(EmpresaDto.Telefono) ||
                    string.IsNullOrEmpty(EmpresaDto.Ciudad) ||
                    string.IsNullOrEmpty(EmpresaDto.Estado) ||
                    string.IsNullOrEmpty(EmpresaDto.FechaFundacion))

                {
                    respuestaDto.idrespuesta = 0;
                    respuestaDto.mensaje = new { codigo = "Datos Faltantes" };
                    return JsonConvert.SerializeObject(respuestaDto);
                }

                respuesta = _sql.EjecutarQuery(json, "gen.spAgregarEmpresa");

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


        public string ActualizarEmpresa(string json)
        {
            RespuestaSpDto respuestaDto = new RespuestaSpDto();
            string respuesta;
            try
            {
                var EmpresaDto = JsonConvert.DeserializeObject<EmpresaDto>(json);
                if (EmpresaDto.EmpresaID == null ||
                    string.IsNullOrEmpty(EmpresaDto.Nombre) ||
                    string.IsNullOrEmpty(EmpresaDto.Email) ||
                    string.IsNullOrEmpty(EmpresaDto.Telefono) ||
                    string.IsNullOrEmpty(EmpresaDto.Ciudad) ||
                    string.IsNullOrEmpty(EmpresaDto.Estado) ||
                    string.IsNullOrEmpty(EmpresaDto.FechaFundacion))

                {
                    respuestaDto.idrespuesta = 0;
                    respuestaDto.mensaje = new { codigo = "Datos Faltantes" };
                    return JsonConvert.SerializeObject(respuestaDto);
                }

                respuesta = _sql.EjecutarQuery(json, "gen.spActualizarEmpresa");

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


        public string EliminarEmpresa(int EmpresaID)
        {
            RespuestaSpDto respuestaDto = new RespuestaSpDto();
            string respuesta;
            try
            {
                var json = JsonConvert.SerializeObject(new { EmpresaID = EmpresaID });
                respuesta = _sql.EjecutarQuery(json, "gen.spEliminarEmpresa");

                if (string.IsNullOrEmpty(respuesta))
                {
                    respuestaDto.idrespuesta = 0;
                    respuestaDto.mensaje = new { codigo = "Empresa No Encontrada" };
                    respuesta = JsonConvert.SerializeObject(respuestaDto);
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


        public string CargarEmpresa(string json)
        {
            RespuestaSpDto respuestaDto = new RespuestaSpDto();
            string respuesta;

            try
            {
                var EmpresaDto = JsonConvert.DeserializeObject<EmpresaDto>(json);

                if (EmpresaDto?.EmpresaID == null)
                {
                    respuestaDto.idrespuesta = 0;
                    respuestaDto.mensaje = new { codigo = "Empresa No Encontrada" };
                    respuesta = JsonConvert.SerializeObject(respuestaDto);
                }
                // Llamada al procedimiento almacenado
                respuesta = _sql.EjecutarQuery(json, "gen.spCargarEmpresa");
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
        
