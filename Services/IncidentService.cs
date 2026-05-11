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
       
            return await _context.Incidents.ToListAsync();

        // BUG: Consulta N+1 problem - no incluye entidades relacionadas
        // BUG: No hay paginación para grandes volúmenes
        // BUG: No hay ordenamiento
    }

    public async Task<Incident?> GetIncidentByIdAsync(int id)
    {
        // BUG: No incluye entidades relacionadas - causa N+1 queries
        return await _context.Incidents.FindAsync(id);
    }

    public async Task<Incident> CreateIncidentAsync(IncidentDto incidentDto)
    {
        // BUG: No hay validación de datos de entrada
        // BUG: No hay manejo de excepciones
        // BUG: No hay transacciones
        
        var incident = new Incident
        {
            Title = incidentDto.Title,
            Description = incidentDto.Description,
            Status = incidentDto.Status,
            Priority = incidentDto.Priority,
            UserId = incidentDto.UserId,
            CategoryId = incidentDto.CategoryId,
            AssignedTo = incidentDto.AssignedTo
        };

        _context.Incidents.Add(incident);
        await _context.SaveChangesAsync();
        
        return incident;
    }

    public async Task<Incident> UpdateIncidentAsync(int id, IncidentDto incidentDto)
    {
        // BUG: No hay validación de existencia del incidente
        // BUG: No hay validación de permisos
        // BUG: No hay auditoría de cambios
        
        var incident = await _context.Incidents.FindAsync(id);
        if (incident == null)
            throw new ArgumentException("Incident not found");

        incident.Title = incidentDto.Title;
        incident.Description = incidentDto.Description;
        incident.Status = incidentDto.Status;
        incident.Priority = incidentDto.Priority;
        incident.CategoryId = incidentDto.CategoryId;
        incident.AssignedTo = incidentDto.AssignedTo;

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
        incident.Status = "Closed";
        incident.ClosedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<DashboardMetricsDto> GetDashboardMetricsAsync()
    {
        // BUG: Consultas ineficientes sin índices
        // BUG: No hay caché para métricas
        // BUG: No hay manejo de errores
        
        var totalIncidents = await _context.Incidents.CountAsync();
        var openIncidents = await _context.Incidents.CountAsync(i => i.Status == "Open");
        var inProgressIncidents = await _context.Incidents.CountAsync(i => i.Status == "In Progress");
        var closedIncidents = await _context.Incidents.CountAsync(i => i.Status == "Closed");
        
        // BUG: No hay agrupación por categoría
        // BUG: No hay agrupación por prioridad
        
        return new DashboardMetricsDto
        {
            TotalIncidents = totalIncidents,
            OpenIncidents = openIncidents,
            InProgressIncidents = inProgressIncidents,
            ClosedIncidents = closedIncidents,
            IncidentsByCategory = new Dictionary<string, int>(),
            IncidentsByPriority = new Dictionary<string, int>()
        };
    }
}
