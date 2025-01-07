namespace SistemaGestion.Interfaces
{
    public interface IEmpleado
    {
        string CargarEmpleado(string json);
        string AgregarEmpleado(string json);
        string ActualizarEmpleado(string json);
        string EliminarEmpleado(string json);
        string ObtenerEmpleado();

    }
}
