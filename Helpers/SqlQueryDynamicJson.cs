using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SistemaGestion.Interfaces;
using SistemaGestion.Models;
using Newtonsoft.Json;
using System.Data.Common;
using SistemaGestion.Dtos;


namespace SistemaGestion.Helpers
{
    public class SqlQueryDynamicJson : ISqlQueryDynamicJson
    {
        private readonly sistemaGestionBD _sistemaGestionBD;

        public SqlQueryDynamicJson(sistemaGestionBD sistemaGestionBD)
        {
            _sistemaGestionBD = sistemaGestionBD;
        }

        public string EjecutarTransaction(string json, string query)
        {
            string respuesta = "";
            SqlConnection connection = new SqlConnection(_sistemaGestionBD.Database.GetDbConnection().ConnectionString);
            SqlCommand cmd;
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            SqlTransaction transaction = connection.BeginTransaction();
            try
            {
                cmd = new SqlCommand(query, connection);
                cmd.CommandType = CommandType.StoredProcedure;

                //Parametros de entrada para el sp
                cmd.Parameters.Add("@json", SqlDbType.NVarChar).Value = json;

                //Parametros de salida
                cmd.Parameters.Add("@respuesta", SqlDbType.NVarChar, int.MaxValue);
                cmd.Parameters["@respuesta"].Direction = ParameterDirection.Output;

                cmd.Transaction = transaction;

                //Ejecutamos query
                cmd.ExecuteNonQuery();

                //Obtenemos y almacenamos respuesta
                respuesta = Convert.ToString(cmd.Parameters["@respuesta"].Value);

                RespuestaSpDto resp = JsonConvert.DeserializeObject<RespuestaSpDto>(respuesta);
                if (resp.idrespuesta == 0)
                {
                    transaction.Rollback();
                    return respuesta;
                }

                transaction.Commit();
            }
            catch (System.Exception ex)
            {
                RespuestaSpDto resp = new RespuestaSpDto();
                resp.idrespuesta = 0;
                resp.mensaje = "apierr";
                respuesta = JsonConvert.SerializeObject(resp);
                transaction.Rollback();
            }
            finally
            {
                connection.Close();
            }
            return respuesta;
        }
        public string EjecutarQuery(string json, string query)
        {
            string respuesta = "";
            try
            {
                DbCommand cmd = _sistemaGestionBD.Database.GetDbConnection().CreateCommand();
                _sistemaGestionBD.Database.OpenConnection();
                cmd.CommandText = query;
                cmd.CommandType = CommandType.StoredProcedure;

                if (!string.IsNullOrEmpty(json))
                {
                    //Paramtros de entrada
                    cmd.Parameters.Add(new SqlParameter("@json", SqlDbType.NVarChar) { Value = json });
                }

                //Parametro de salida
                cmd.Parameters.Add(new SqlParameter("@respuesta", SqlDbType.NVarChar, int.MaxValue));
                cmd.Parameters["@respuesta"].Direction = ParameterDirection.Output;

                cmd.CommandTimeout = 240;
                                         
                cmd.ExecuteNonQuery();

                //Almacenamiento de respuesta
                respuesta = Convert.ToString(cmd.Parameters["@respuesta"].Value);
            }
            catch (System.Exception ex)
            {
                respuesta = "";
            }
            finally
            {
                _sistemaGestionBD.Database.CloseConnection();
            }
            return respuesta;
        }
    }
}
