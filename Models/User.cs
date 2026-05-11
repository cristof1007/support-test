using System.ComponentModel.DataAnnotations;

namespace IncidentManagementSystem.Models;

public class User
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Username { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    [StringLength(255)]
    public string Email { get; set; } = string.Empty;
    
    public bool IsActive { get; set; } = true;
    
    // BUG: No hay navegación inversa - causa problemas de lazy loading
    // public virtual ICollection<Incident> Incidents { get; set; } = new List<Incident>();
    // public virtual ICollection<Incident> AssignedIncidents { get; set; } = new List<Incident>();
    
    // BUG: Falta campo de auditoría
    // public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}
