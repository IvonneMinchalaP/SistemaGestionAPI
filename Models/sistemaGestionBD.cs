using Microsoft.EntityFrameworkCore;


namespace SistemaGestion.Models
{
    public class sistemaGestionBD : DbContext
    {
        public sistemaGestionBD(DbContextOptions <sistemaGestionBD> options) : base(options)
        {
        }
    }
}
