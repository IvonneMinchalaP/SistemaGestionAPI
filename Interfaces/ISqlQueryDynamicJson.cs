namespace SistemaGestion.Interfaces
{
    public interface ISqlQueryDynamicJson
    {
        string EjecutarQuery(string json, string query);
        string EjecutarTransaction(string json, string query);
    }
}
