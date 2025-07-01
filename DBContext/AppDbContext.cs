using Microsoft.EntityFrameworkCore;
using NFCEApp.Models;

namespace NFCEApp.DBContext
{
    public class AppDbContext : DbContext
    {
        //public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<NotaFiscal> NotasFiscais { get; set; }
        public DbSet<Produto> Produtos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string conexionDb = Path.Combine(FileSystem.AppDataDirectory, "local.db");
            optionsBuilder.UseSqlite($"Filename={conexionDb}");
        }
    }
}
