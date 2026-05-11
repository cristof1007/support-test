using System.ComponentModel.DataAnnotations;

namespace IncidentManagementSystem.DTOs;

public class IncidentDto
{
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;
    
    [StringLength(1000)]
    public string? Description { get; set; }
    
    // BUG: No hay validación de enum - debería ser un enum o tener validación
    public string Status { get; set; } = "Open";
    
    // BUG: No hay validación de enum - debería ser un enum o tener validación
    public string Priority { get; set; } = "Medium";
    
    [Range(1, int.MaxValue)]
    public int UserId { get; set; }
    
    [Range(1, int.MaxValue)]
    public int CategoryId { get; set; }
    
    public int? AssignedTo { get; set; }
}
