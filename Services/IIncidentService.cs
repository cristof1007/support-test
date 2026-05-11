using IncidentManagementSystem.DTOs;
using IncidentManagementSystem.Models;

namespace IncidentManagementSystem.Services;

public interface IIncidentService
{
    Task<IEnumerable<Incident>> GetAllIncidentsAsync();
    Task<Incident?> GetIncidentByIdAsync(int id);
    Task<Incident> CreateIncidentAsync(IncidentDto incidentDto);
    Task<Incident> UpdateIncidentAsync(int id, IncidentDto incidentDto);
    Task<bool> CloseIncidentAsync(int id);
    Task<DashboardMetricsDto> GetDashboardMetricsAsync();
}
