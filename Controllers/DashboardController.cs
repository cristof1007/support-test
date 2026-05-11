using Microsoft.AspNetCore.Mvc;
using IncidentManagementSystem.DTOs;
using IncidentManagementSystem.Services;

namespace IncidentManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IncidentService _incidentService;

    public DashboardController(IncidentService incidentService)
    {
        _incidentService = incidentService;
    }

    [HttpGet("metrics")]
    public async Task<ActionResult<DashboardMetricsDto>> GetMetrics()
    {
        try
        {
            // BUG: No hay caché para métricas que cambian poco
            // BUG: No hay validación de permisos
            // BUG: No hay manejo de timeouts
            
            var metrics = await _incidentService.GetDashboardMetricsAsync();
            return Ok(metrics);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }
}
