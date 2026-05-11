using Microsoft.EntityFrameworkCore;
using IncidentManagementSystem.Models;

namespace IncidentManagementSystem.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Incident> Incidents { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // BUG: No hay configuración de relaciones - causa problemas de foreign keys
        // BUG: No hay índices para optimizar consultas
        // BUG: No hay constraints de validación
        
        modelBuilder.Entity<Incident>()
            .HasOne(i => i.User)
            .WithMany()
            .HasForeignKey(i => i.UserId)
            .OnDelete(DeleteBehavior.Restrict);
            
        modelBuilder.Entity<Incident>()
            .HasOne(i => i.Category)
            .WithMany()
            .HasForeignKey(i => i.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
            
        modelBuilder.Entity<Incident>()
            .HasOne(i => i.AssignedUser)
            .WithMany()
            .HasForeignKey(i => i.AssignedTo)
            .OnDelete(DeleteBehavior.Restrict);
            
        // BUG: Falta configuración de índices para optimizar consultas frecuentes
        // BUG: Falta configuración de constraints para Status y Priority
        // BUG: Falta configuración de auditoría
    }
}
