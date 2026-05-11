namespace IncidentManagementSystem.DTOs;

public class DashboardMetricsDto
{
    public int TotalIncidents { get; set; }
    public int OpenIncidents { get; set; }
    public int InProgressIncidents { get; set; }
    public int ClosedIncidents { get; set; }
    public Dictionary<string, int> IncidentsByCategory { get; set; } = new();
    public Dictionary<string, int> IncidentsByPriority { get; set; } = new();
}
