namespace SistemaGestion.Interfaces
{
    public interface IUsuario
    {
        string RegistrarUsuario(string json);
        string ConsultarUsuario(string json);
        string ActualizarUsuario(string json);
        string IniciarSesion(string json);
    }
}
