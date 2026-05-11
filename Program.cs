using Microsoft.EntityFrameworkCore;
using IncidentManagementSystem.Data;
using IncidentManagementSystem.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//CAMBIO: Validación defensiva de connection string
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// BUG: No se registra la interfaz, solo la implementación
//CAMBIO: Se registra la abstracción para seguir DIP (Dependency Inversion Principle)
builder.Services.AddScoped<IIncidentService, IncidentService>();

//CAMBIO: DatabaseService registrado correctamente (si depende de DI)
builder.Services.AddScoped<DatabaseService>();

// BUG: No se agrega logging
//CAMBIO: Se habilita logging estructurado
builder.Services.AddLogging();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//CAMBIO: Se agrega middleware de routing (buena práctica moderna)
app.UseRouting();

app.UseAuthorization();

app.MapControllers();

//CAMBIO: Corrección de migraciones seguras al iniciar la app
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    //CAMBIO: Manejo de errores en migración para evitar crash en startup
    try
    {
        await context.Database.MigrateAsync();
        await DbInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Error applying migrations or initializing database.");
        throw;
    }
}

app.Run();