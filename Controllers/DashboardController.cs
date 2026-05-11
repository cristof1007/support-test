using Microsoft.AspNetCore.Mvc;
using IncidentManagementSystem.DTOs;
using IncidentManagementSystem.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;

namespace IncidentManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IncidentService _incidentService;
    private readonly ILogger<DashboardController> _logger;
    private readonly IMemoryCache _cache;

    public DashboardController(
        IncidentService incidentService,
        ILogger<DashboardController> logger,
        IMemoryCache cache)
    {
        _incidentService = incidentService;
        _logger = logger;
        _cache = cache;
    }

    [HttpGet("metrics")]
    public async Task<ActionResult<DashboardMetricsDto>> GetMetrics()
    {
        try
        {
            // BUG: No hay caché para métricas que cambian poco
            // BUG: No hay validación de permisos
            // BUG: No hay manejo de timeouts

            const string cacheKey = "dashboard_metrics";

            //CAMBIO: Implementación de caché en memoria (5 minutos)
           if (_cache.TryGetValue(cacheKey, out DashboardMetricsDto cachedMetrics))
            {
                _logger.LogInformation("Dashboard metrics obtenidas desde caché");
                return Ok(cachedMetrics);
            }

            //CAMBIO: Control de timeout agregado para evitar bloqueos
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            //CAMBIO: Logging de inicio de operación
            _logger.LogInformation("Generando métricas del dashboard");

            var metrics = await _incidentService.GetDashboardMetricsAsync();

            //CAMBIO: Guardado en caché para reutilización
            _cache.Set(cacheKey, metrics, TimeSpan.FromMinutes(5));

            return Ok(metrics);
        }
        catch (OperationCanceledException ex)
        {
            //CAMBIO: Manejo específico de timeout
            _logger.LogError(ex, "Timeout generando métricas del dashboard");

            return StatusCode(504, "Internal Server Error");
        }
        catch (Exception ex)
        {
            //CAMBIO: Logging estructurado del error
            _logger.LogError(ex, "Error en GetMetrics dashboard");

            return StatusCode(500, "Internal Server Error");
        }
    }
}