namespace SistemaGestion.Interfaces
{
    public interface IEmpleado
    {
        string CargarEmpleado(int EmpleadoID);
        string AgregarEmpleado(string json);
        string ConsultarEmpleado(int EmpleadoID);
        string ActualizarEmpleado(string json);
        string EliminarEmpleado(int EmpleadoID);
        string ObtenerEmpleado();

    }
}
