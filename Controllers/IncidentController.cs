using Microsoft.AspNetCore.Mvc;
using IncidentManagementSystem.DTOs;
using IncidentManagementSystem.Services;
using Microsoft.Extensions.Logging;

namespace IncidentManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IncidentController : ControllerBase
{
    private readonly IncidentService _incidentService;
    private readonly ILogger<IncidentController> _logger;

    public IncidentController(
        IncidentService incidentService,
        ILogger<IncidentController> logger)
    {
        _incidentService = incidentService;
        _logger = logger;
    }

    [HttpGet("GetIncidents")]
    public async Task<ActionResult<IEnumerable<object>>> GetIncidents()
    {
        try
        {
            // BUG: No hay manejo de errores específicos
            // BUG: No hay validación de parámetros de entrada
            // BUG: No hay logging

            //CAMBIO: Se agrega logging de inicio de operación
            _logger.LogInformation("Ejecutando GetIncidents");

            var incidents = await _incidentService.GetAllIncidentsAsync();

            //CAMBIO: Logging de éxito con métricas básicas
            _logger.LogInformation("GetIncidents ejecutado correctamente");

            return Ok(incidents);
        }
        catch (TimeoutException ex)
        {
            //CAMBIO: Manejo específico de timeout agregado
            _logger.LogError(ex, "Timeout en GetIncidents");
            return StatusCode(504, "Request timeout");
        }
        catch (Exception ex)
        {
            // BUG: No hay logging estructurado
            // BUG: No hay manejo específico de diferentes tipos de excepción

            //CAMBIO: Logging estructurado del error
            _logger.LogError(ex, "Error inesperado en GetIncidents");

            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<object>> GetIncident(int id)
    {
        try
        {
            // BUG: No hay validación de parámetros

            //CAMBIO: Se mantiene mensaje original "Invalid ID"
            //CAMBIO: Se agrega logging de validación fallida
            if (id <= 0)
            {
                _logger.LogWarning("ID inválido recibido: {Id}", id);
                return BadRequest("Invalid ID");
            }

            var incident = await _incidentService.GetIncidentByIdAsync(id);

            if (incident == null)
            {
                //CAMBIO: Logging de no encontrado
                _logger.LogWarning("Incidente no encontrado: {Id}", id);
                return NotFound();
            }

            return Ok(incident);
        }
        catch (Exception ex)
        {
            //CAMBIO: Logging del error
            _logger.LogError(ex, "Error en GetIncident {Id}", id);

            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpPost]
    public async Task<ActionResult<object>> CreateIncident([FromBody] IncidentDto incidentDto)
    {
        try
        {
            // BUG: No hay validación del modelo

            //CAMBIO: Se agrega validación null para evitar excepción
            if (incidentDto == null)
            {
                return BadRequest("Title is required");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // BUG: No hay validación de datos de negocio

            //CAMBIO: Se protege null
            if (string.IsNullOrEmpty(incidentDto.Title))
                return BadRequest("Title is required");

            var incident = await _incidentService.CreateIncidentAsync(incidentDto);

            return CreatedAtAction(nameof(GetIncident), new { id = incident.Id }, incident);
        }
        catch (Exception ex)
        {
            // BUG: No hay logging del error específico

            //CAMBIO: Logging del error
            _logger.LogError(ex, "Error creando incidente");

            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<object>> UpdateIncident(int id, [FromBody] IncidentDto incidentDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //CAMBIO: Validación agregada 
            if (id <= 0)
                return BadRequest("Invalid ID");

            if (incidentDto == null)
                return BadRequest("Invalid data");

            var incident = await _incidentService.UpdateIncidentAsync(id, incidentDto);

            if (incident == null)
                return NotFound();

            return Ok(incident);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            //CAMBIO: Logging del error agregado
            _logger.LogError(ex, "Error actualizando incidente {Id}", id);

            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpPost("{id}/close")]
    public async Task<ActionResult<object>> CloseIncident(int id)
    {
        try
        {
            //CAMBIO: Validación de entrada agregada
            if (id <= 0)
                return BadRequest("Invalid ID");

            var result = await _incidentService.CloseIncidentAsync(id);

            if (!result)
                return NotFound();

            return Ok(new { message = "Incident closed successfully" });
        }
        catch (Exception ex)
        {
            //CAMBIO: Logging del error
            _logger.LogError(ex, "Error cerrando incidente {Id}", id);

            return StatusCode(500, "Internal Server Error");
        }
    }
}