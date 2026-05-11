using System.ComponentModel.DataAnnotations;

namespace IncidentManagementSystem.Models;

public class Incident
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;
    
    [StringLength(1000)]
    public string? Description { get; set; }
    
    // BUG: No hay validación de enum - debería ser un enum o tener validación
    public string Status { get; set; } = "Open";
    
    // BUG: No hay validación de enum - debería ser un enum o tener validación
    public string Priority { get; set; } = "Medium";
    
    // BUG: No hay validación de foreign key
    public int UserId { get; set; }
    public virtual User User { get; set; } = null!;
    
    // BUG: No hay validación de foreign key
    public int CategoryId { get; set; }
    public virtual Category Category { get; set; } = null!;
    
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    
    // BUG: No hay lógica de negocio para esta fecha
    public DateTime? ClosedDate { get; set; }
    
    // BUG: No hay validación de foreign key
    public int? AssignedTo { get; set; }
    public virtual User? AssignedUser { get; set; }
    
    // BUG: Falta campo ModifiedDate para auditoría
    // public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;
}
