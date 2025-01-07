using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using SistemaGestion.Dtos;
using SistemaGestion.Interfaces;
using System.Data;

namespace SistemaGestion.Servicios
{
    public class ServiceEmpleado : IEmpleado
    {
        private readonly ISqlQueryDynamicJson _sql;

        public ServiceEmpleado(ISqlQueryDynamicJson sql)
        {
            _sql = sql;
        }
        public string ObtenerEmpleado()
        {
            RespuestaSpDto respuestaDto = new RespuestaSpDto();
            string respuesta;

            try
            {
                respuesta = _sql.EjecutarQuery("{}", "gen.spObtenerEmpleados");
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
        public string CargarEmpleado(string json)
        {
            RespuestaSpDto respuestaDto = new RespuestaSpDto();
            string respuesta;
            try
            {
                var EmpleadoDto = JsonConvert.DeserializeObject<EmpleadoDto>(json);

                if (EmpleadoDto?.EmpleadoID == null)
                {
                    respuestaDto.idrespuesta = 0;
                    respuestaDto.mensaje = new { codigo = "Empleado No Encontrada" };
                    respuesta = JsonConvert.SerializeObject(respuestaDto);
                }
                // Llamada al procedimiento almacenado
                respuesta = _sql.EjecutarQuery(json, "gen.spCargarEmpleado");
            }
            catch (Exception ex)
            {
                respuestaDto.idrespuesta = 0;
                respuestaDto.mensaje = new { codigo = "Error Interno", detalle = ex.Message };
                respuesta = JsonConvert.SerializeObject(respuestaDto);
            }

            return respuesta;
        }

        public string AgregarEmpleado(string json)
        {
            RespuestaSpDto respuestaDto = new RespuestaSpDto();
            string respuesta;
            try
            {
                var EmpleadoDto = JsonConvert.DeserializeObject<EmpleadoDto>(json);
                if (string.IsNullOrEmpty(EmpleadoDto.Nombre) ||
                    string.IsNullOrEmpty(EmpleadoDto.Email) ||
                    string.IsNullOrEmpty(EmpleadoDto.Puesto) ||
                    string.IsNullOrEmpty(EmpleadoDto.Telefono) ||
                    string.IsNullOrEmpty(EmpleadoDto.FechaIngreso))

                {
                    respuestaDto.idrespuesta = 0;
                    respuestaDto.mensaje = new { codigo = "Datos Faltantes" };
                    return JsonConvert.SerializeObject(respuestaDto);
                }

                respuesta = _sql.EjecutarQuery(json, "gen.spAgregarEmpleado");

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


        public string ActualizarEmpleado(string json)
        {
            RespuestaSpDto respuestaDto = new RespuestaSpDto();
            string respuesta;
            try
            {
                var EmpleadoDto = JsonConvert.DeserializeObject<EmpleadoDto>(json);
                if (EmpleadoDto.EmpleadoID == null ||
                    string.IsNullOrEmpty(EmpleadoDto.Nombre) ||
                    string.IsNullOrEmpty(EmpleadoDto.Email) ||
                    string.IsNullOrEmpty(EmpleadoDto.Puesto) ||
                    string.IsNullOrEmpty(EmpleadoDto.Telefono) ||
                    string.IsNullOrEmpty(EmpleadoDto.FechaIngreso))

                {
                    respuestaDto.idrespuesta = 0;
                    respuestaDto.mensaje = new { codigo = "Datos Faltantes" };
                    return JsonConvert.SerializeObject(respuestaDto);
                }

                respuesta = _sql.EjecutarQuery(json, "gen.spActualizarEmpleado");

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

        public string EliminarEmpleado(string json)
        {
            RespuestaSpDto respuestaDto = new RespuestaSpDto();
            string respuesta;
            try
            {
                var EmpleadoDto = JsonConvert.DeserializeObject<EmpleadoDto>(json);

                if (EmpleadoDto?.EmpleadoID == null)
                {
                    respuestaDto.idrespuesta = 0;
                    respuestaDto.mensaje = new { codigo = "empleado No Encontrada" };
                    respuesta = JsonConvert.SerializeObject(respuestaDto);
                }
                // Llamada al procedimiento almacenado
                respuesta = _sql.EjecutarQuery(json, "gen.spEliminarEmpleado");
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