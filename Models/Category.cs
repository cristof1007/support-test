using System.ComponentModel.DataAnnotations;

namespace IncidentManagementSystem.Models;

public class Category
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [StringLength(500)]
    public string? Description { get; set; }
    
    // BUG: No hay navegación inversa - causa problemas de lazy loading
    // public virtual ICollection<Incident> Incidents { get; set; } = new List<Incident>();
    
    // BUG: Falta campo de auditoría
    // public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    
     public bool IsActive { get; set; } = true;
}
