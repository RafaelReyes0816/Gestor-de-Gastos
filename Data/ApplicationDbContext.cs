using Microsoft.EntityFrameworkCore;
using Gestor_Gastos.Models;

namespace Gestor_Gastos.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Gasto> Gastos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de Usuario
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Configuración de Gasto
            modelBuilder.Entity<Gasto>(entity =>
            {
                entity.HasOne(g => g.Usuario)
                    .WithMany(u => u.Gastos)
                    .HasForeignKey(g => g.UsuarioId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(g => g.Categoria)
                    .WithMany(c => c.Gastos)
                    .HasForeignKey(g => g.CategoriaId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(g => g.UsuarioId);
                entity.HasIndex(g => g.FechaGasto);
                entity.HasIndex(g => g.CategoriaId);
            });
        }
    }
}

