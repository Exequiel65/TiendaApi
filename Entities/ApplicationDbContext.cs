using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> opciones) : base(opciones)
        {
        }

        //Escribir modelos
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Marca> Marcas { get; set; }
        public DbSet<Categoria> Categorias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Producto>(entity =>
            {
                entity.ToTable("Producto");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Nombre)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(p => p.Precio)
                    .HasColumnType("decimal(18,2)");

                entity.HasOne(p => p.Marca)
                    .WithMany(p => p.Productos)
                    .HasForeignKey(p => p.MarcaId);
                entity.HasOne(p => p.Categoria)
                    .WithMany(p => p.Productos)
                    .HasForeignKey(p => p.CategoriaId);
            });

            modelBuilder.Entity<Marca>(entity =>
            {
                entity.ToTable("Marca");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Nombre)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.ToTable("Categoria");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Nombre)
                    .IsRequired()
                    .HasMaxLength(100);
            });

        }
    }
}