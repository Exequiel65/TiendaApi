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
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }


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

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("Usuario");
                entity.Property(p => p.Id)
                    .IsRequired();
                entity.Property(p => p.Nombres)
                    .IsRequired()
                    .HasMaxLength(200);
                entity.Property(p => p.ApellidoPaterno)
                    .IsRequired()
                    .HasMaxLength(200);
                entity.Property(p => p.Username)
                    .IsRequired()
                    .HasMaxLength(50);
                entity.Property(p => p.Email)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasMany(p => p.Roles)
                    .WithMany(p => p.Usuarios)
                    .UsingEntity<UsuariosRoles>(
                        j => j
                            .HasOne(pt => pt.Rol)
                            .WithMany(t => t.UsuariosRoles)
                            .HasForeignKey(pt => pt.RoleId),
                        j => j
                            .HasOne(pt => pt.Usuario)
                            .WithMany(p => p.UsuariosRoles)
                            .HasForeignKey(pt => pt.UsuarioId),
                        j =>
                        {
                            j.HasKey(t => new { t.UsuarioId, t.RoleId });
                        });
            });

            modelBuilder.Entity<Rol>(entity =>
            {
                entity.ToTable("Rol");
                entity.Property(p => p.Id)
                    .IsRequired();
                entity.Property(p => p.Nombre)
                    .IsRequired()
                    .HasMaxLength(200);
            });

        }
    }
}