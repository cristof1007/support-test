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
    //CAMBIO: Se habilita relación inversa para permitir navegación bidireccional
    // y evitar problemas en consultas relacionadas con Incidents
    public virtual ICollection<Incident> Incidents { get; set; } = new List<Incident>();

    // BUG: Falta campo de auditoría
    //CAMBIO: Se agrega CreatedDate para control de creación de registros
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    //CAMBIO: Mejora de control de estado
    // Permite activar/desactivar categorías sin eliminarlas
    public bool IsActive { get; set; } = true;
}
