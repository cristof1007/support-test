using Microsoft.AspNetCore.Mvc;
using IncidentManagementSystem.DTOs;
using IncidentManagementSystem.Services;

namespace IncidentManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IncidentController : ControllerBase
{
    private readonly IncidentService _incidentService;

    public IncidentController(IncidentService incidentService)
    {
        _incidentService = incidentService;
    }

    [HttpGet("GetIncidents")]
    public async Task<ActionResult<IEnumerable<object>>> GetIncidents()
    {
        try
        {
            // BUG: No hay manejo de errores específicos
            // BUG: No hay validación de parámetros de entrada
            // BUG: No hay logging
            
            var incidents = await _incidentService.GetAllIncidentsAsync();
            return Ok(incidents);
        }
        catch (Exception ex)
        {
            // BUG: No hay logging estructurado
            // BUG: No hay manejo específico de diferentes tipos de excepción
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<object>> GetIncident(int id)
    {
        try
        {
            // BUG: No hay validación de parámetros
            if (id <= 0)
                return BadRequest("Invalid ID");

            var incident = await _incidentService.GetIncidentByIdAsync(id);
            if (incident == null)
                return NotFound();

            return Ok(incident);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpPost]
    public async Task<ActionResult<object>> CreateIncident([FromBody] IncidentDto incidentDto)
    {
        try
        {
            // BUG: No hay validación del modelo
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // BUG: No hay validación de datos de negocio
            if (string.IsNullOrEmpty(incidentDto.Title))
                return BadRequest("Title is required");

            var incident = await _incidentService.CreateIncidentAsync(incidentDto);
            return CreatedAtAction(nameof(GetIncident), new { id = incident.Id }, incident);
        }
        catch (Exception ex)
        {
            // BUG: No hay logging del error específico
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

            var incident = await _incidentService.UpdateIncidentAsync(id, incidentDto);
            return Ok(incident);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpPost("{id}/close")]
    public async Task<ActionResult<object>> CloseIncident(int id)
    {
        try
        {
            var result = await _incidentService.CloseIncidentAsync(id);
            if (!result)
                return NotFound();

            return Ok(new { message = "Incident closed successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }
}
