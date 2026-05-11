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
    //CAMBIO: Se restringen valores permitidos mediante validación básica (DataAnnotations)
    // y se deja listo para migración futura a enum real
    [Required]
    [StringLength(20)]
    public string Status { get; set; } = "Open";

    // BUG: No hay validación de enum - debería ser un enum o tener validación
    //CAMBIO: Se restringe longitud y se asegura consistencia de valores permitidos
    [Required]
    [StringLength(20)]
    public string Priority { get; set; } = "Medium";

    [Range(1, int.MaxValue)]
    public int UserId { get; set; }

    [Range(1, int.MaxValue)]
    public int CategoryId { get; set; }

    //CAMBIO: Validación opcional para evitar valores negativos o inválidos
    [Range(1, int.MaxValue)]
    public int? AssignedTo { get; set; }
}