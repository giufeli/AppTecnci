using Microsoft.EntityFrameworkCore;
using AppTecnici.Shared.Models;

namespace AppTecnici.Server.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Impianto> Impianti { get; set; }
        public DbSet<Intervento> Interventi { get; set; }
    }
}