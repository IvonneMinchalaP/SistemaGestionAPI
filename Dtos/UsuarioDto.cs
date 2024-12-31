namespace SistemaGestion.Dtos
{
    public class UsuarioDto
    {
        public int? UsuarioID { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Email { get; set; }
        public string? Contrasena { get; set; }
        public string? Genero { get; set; }

        public string? FechaNacimiento { get; set; }


    }
}
