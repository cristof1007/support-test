using IncidentManagementSystem.Models;

namespace IncidentManagementSystem.Data;

public static class DbInitializer
{
    public static async Task Initialize(ApplicationDbContext context)
    {
        // Asegurar que la base de datos esté creada
        await context.Database.EnsureCreatedAsync();

        // Verificar si ya hay datos
        if (context.Users.Any())
            return;

        // Crear usuarios de prueba
        var users = new[]
        {
            new User { Username = "admin", Email = "admin@techsolutions.com", IsActive = true },
            new User { Username = "user1", Email = "user1@techsolutions.com", IsActive = true },
            new User { Username = "user2", Email = "user2@techsolutions.com", IsActive = true },
            new User { Username = "support1", Email = "support1@techsolutions.com", IsActive = true },
            new User { Username = "support2", Email = "support2@techsolutions.com", IsActive = true }
        };

        context.Users.AddRange(users);
        await context.SaveChangesAsync();

        // Crear categorías de prueba
        var categories = new[]
        {
            new Category { Name = "Hardware", Description = "Problemas relacionados con equipos físicos", IsActive = true },
            new Category { Name = "Software", Description = "Problemas relacionados con aplicaciones y sistemas", IsActive = true },
            new Category { Name = "Network", Description = "Problemas de conectividad y red", IsActive = true },
            new Category { Name = "Database", Description = "Problemas relacionados con bases de datos", IsActive = true },
            new Category { Name = "Security", Description = "Problemas de seguridad y acceso", IsActive = true }
        };

        context.Categories.AddRange(categories);
        await context.SaveChangesAsync();

        // Crear incidentes de prueba
        var incidents = new[]
        {
            new Incident
            {
                Title = "PC no enciende",
                Description = "La computadora no responde al botón de encendido, verificar fuente de poder",
                Status = "Open",
                Priority = "High",
                UserId = users[1].Id,
                CategoryId = categories[0].Id,
                AssignedTo = users[3].Id
            },
            new Incident
            {
                Title = "Error en login del sistema",
                Description = "No puedo acceder al sistema, aparece error de credenciales inválidas",
                Status = "In Progress",
                Priority = "Medium",
                UserId = users[2].Id,
                CategoryId = categories[1].Id,
                AssignedTo = users[3].Id
            }
        };

        context.Incidents.AddRange(incidents);
        await context.SaveChangesAsync();
    }
}
