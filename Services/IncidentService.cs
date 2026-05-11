using Microsoft.EntityFrameworkCore;
using IncidentManagementSystem.Data;
using IncidentManagementSystem.DTOs;
using IncidentManagementSystem.Models;

namespace IncidentManagementSystem.Services;

public class IncidentService : IIncidentService
{
    private readonly ApplicationDbContext _context;

    public IncidentService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Incident>> GetAllIncidentsAsync()
    {
        // BUG: Consulta N+1 problem - no incluye entidades relacionadas
        // BUG: No hay paginación para grandes volúmenes
        // BUG: No hay ordenamiento

        //CAMBIO: Se incluyen relaciones necesarias para evitar N+1 problem
        //CAMBIO: Se agrega ordenamiento por fecha (más recientes primero)
       

        return await _context.Incidents
            .Include(i => i.User)
            .Include(i => i.Category)
            .Include(i => i.AssignedUser)
            .OrderByDescending(i => i.CreatedDate)
            .ToListAsync();
    }

    public async Task<Incident?> GetIncidentByIdAsync(int id)
    {
        // BUG: No incluye entidades relacionadas - causa N+1 queries

        //CAMBIO: Se incluyen relaciones para evitar consultas adicionales
        return await _context.Incidents
            .Include(i => i.User)
            .Include(i => i.Category)
            .Include(i => i.AssignedUser)
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<Incident> CreateIncidentAsync(IncidentDto incidentDto)
    {
        // BUG: No hay validación de datos de entrada
        // BUG: No hay manejo de excepciones
        // BUG: No hay transacciones

        //CAMBIO:Validación básica defensiva
        if (incidentDto == null)
            throw new ArgumentNullException(nameof(incidentDto));

        //CAMBIO: Uso de transacción para consistencia de datos
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var incident = new Incident
            {
                Title = incidentDto.Title,
                Description = incidentDto.Description,
                Status = incidentDto.Status,
                Priority = incidentDto.Priority,
                UserId = incidentDto.UserId,
                CategoryId = incidentDto.CategoryId,
                AssignedTo = incidentDto.AssignedTo,
                CreatedDate = DateTime.UtcNow
            };

            _context.Incidents.Add(incident);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return incident;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<Incident> UpdateIncidentAsync(int id, IncidentDto incidentDto)
    {
        // BUG: No hay validación de existencia del incidente
        // BUG: No hay validación de permisos
        // BUG: No hay auditoría de cambios

        var incident = await _context.Incidents.FindAsync(id);
        if (incident == null)
            throw new ArgumentException("Incident not found");

        //CAMBIO: Auditoría básica de modificación
        incident.Title = incidentDto.Title;
        incident.Description = incidentDto.Description;
        incident.Status = incidentDto.Status;
        incident.Priority = incidentDto.Priority;
        incident.CategoryId = incidentDto.CategoryId;
        incident.AssignedTo = incidentDto.AssignedTo;

        //CAMBIO: Registro de modificación
        incident.ModifiedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return incident;
    }

    public async Task<bool> CloseIncidentAsync(int id)
    {
        // BUG: Lógica incorrecta - no valida el estado actual
        // BUG: No hay validación de permisos
        // BUG: No hay auditoría

        var incident = await _context.Incidents.FindAsync(id);
        if (incident == null)
            return false;

        // BUG: No valida si el incidente ya está cerrado

        //CAMBIO: Validación de estado actual
        if (incident.Status == "Closed")
            return false;

        incident.Status = "Closed";
        incident.ClosedDate = DateTime.UtcNow;

        //CAMBIO: Auditoría de cierre
        incident.ModifiedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<DashboardMetricsDto> GetDashboardMetricsAsync()
    {
        // BUG: Consultas ineficientes sin índices
        // BUG: No hay caché para métricas
        // BUG: No hay manejo de errores

        //CAMBIO: Agrupación en una sola consulta para optimizar performance
        var incidents = await _context.Incidents
            .AsNoTracking()
            .ToListAsync();

        var totalIncidents = incidents.Count;
        var openIncidents = incidents.Count(i => i.Status == "Open");
        var inProgressIncidents = incidents.Count(i => i.Status == "In Progress");
        var closedIncidents = incidents.Count(i => i.Status == "Closed");

        // BUG: No hay agrupación por categoría
        // BUG: No hay agrupación por prioridad

        //CAMBIO: Agrupación eficiente en memoria (evita múltiples queries)
        var incidentsByCategory = incidents
            .GroupBy(i => i.CategoryId.ToString())
            .ToDictionary(g => g.Key, g => g.Count());

        var incidentsByPriority = incidents
            .GroupBy(i => i.Priority)
            .ToDictionary(g => g.Key, g => g.Count());

        return new DashboardMetricsDto
        {
            TotalIncidents = totalIncidents,
            OpenIncidents = openIncidents,
            InProgressIncidents = inProgressIncidents,
            ClosedIncidents = closedIncidents,
            IncidentsByCategory = incidentsByCategory,
            IncidentsByPriority = incidentsByPriority
        };
    }
}