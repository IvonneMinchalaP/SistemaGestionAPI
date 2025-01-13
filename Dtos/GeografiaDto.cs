namespace SistemaGestion.Dtos
{
    public class GeografiaDto
    {
        public class ParroquiaDto
        {
            public int id { get; set; }
            public int parentId { get; set; }
            public string text { get; set; }
        }

        public class CiudadDto
        {
            public int id { get; set; }
            public int parentId { get; set; }
            public string text { get; set; }
            public List<ParroquiaDto> items { get; set; }
        }

        public class ProvinciaDto
        {
            public int id { get; set; }
            public int parentId { get; set; }
            public string text { get; set; }
            public List<CiudadDto> items { get; set; }
        }

        public class PaisDto
        {
            public int id { get; set; }
            public string text { get; set; }
            public List<ProvinciaDto> items { get; set; }
        }
    }

}

