
using Microsoft.EntityFrameworkCore;
using ProjetoAWS.lib.Models;

namespace ProjetoAWS.lib.Data
{
    public class AWSContext : DbContext
    {
        public AWSContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Usuario> Usuarios { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Usuarios
            modelBuilder.Entity<Usuario>().ToTable("aws_usuarios");
            modelBuilder.Entity<Usuario>().HasKey(x => x.Id);
        }

    }
}