namespace SistemaGestion.Dtos
{
    public class RespuestaSpDto
    {
        public int idrespuesta { get; set; }
        public object? mensaje { get; set; }
        public List<GeografiaDto.PaisDto> geograficas { get; set; }

    }
}
