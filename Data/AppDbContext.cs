using Microsoft.EntityFrameworkCore;
using SuperMarketAPI.Models;


namespace SuperMarketAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<Producto> Productos { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //Confirguracion de Producto 
        modelBuilder.Entity<Producto>(entity =>
        {
            entity.Property(p => p.Precio).HasColumnType("decimal(10,2)");
            entity.Property(p => p.Nombre).IsRequired().HasMaxLength(100);
            entity.HasOne(p => p.Categoria)
                .WithMany(c => c.Productos)
                .HasForeignKey(p => p.CategoriaId);
        });

        // Configuración de Categoria
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.Property(c => c.Nombre).HasMaxLength(50).IsRequired();
            entity.HasIndex(c => c.Nombre).IsUnique();
        });

        // Configuración de Usuario
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.Property(u => u.Nombre).HasMaxLength(100).IsRequired();
            entity.Property(u => u.Email).HasMaxLength(150).IsRequired();
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.Rol).HasMaxLength(20);
        });

        // Configuración de RefreshToken
        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.Property(r => r.Token).HasMaxLength(200).IsRequired();
            entity.HasIndex(r => r.Token).IsUnique();
            entity.HasOne(r => r.Usuario)
                  .WithMany(u => u.RefreshTokens)
                  .HasForeignKey(r => r.UsuarioId);
        });
    }
}
