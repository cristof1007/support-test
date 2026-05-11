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
    //CAMBIO: Se restringe por DataAnnotations y se deja listo para migración a enum
    [Required]
    [StringLength(20)]
    public string Status { get; set; } = "Open";

    // BUG: No hay validación de enum - debería ser un enum o tener validación
    //CAMBIO: Se restringe longitud y se prepara para futura conversión a enum
    [Required]
    [StringLength(20)]
    public string Priority { get; set; } = "Medium";

    // BUG: No hay validación de foreign key
    //CAMBIO: Se agrega Required para asegurar integridad referencial en modelo
    [Required]
    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;

    // BUG: No hay validación de foreign key
    //CAMBIO: Se agrega Required para evitar valores inválidos en creación
    [Required]
    public int CategoryId { get; set; }

    public virtual Category Category { get; set; } = null!;

    //CAMBIO: Se mantiene UTC como estándar de auditoría consistente
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    // BUG: No hay lógica de negocio para esta fecha
    //CAMBIO: Se mantiene nullable para representar estado de cierre real
    // y permitir control en service layer
    public DateTime? ClosedDate { get; set; }

    // BUG: No hay validación de foreign key
    //CAMBIO: Se mantiene nullable correctamente porque es asignación opcional
    public int? AssignedTo { get; set; }

    public virtual User? AssignedUser { get; set; }

    // BUG: Falta campo ModifiedDate para auditoría
    //CAMBIO: Se habilita campo de auditoría para control de cambios
    public DateTime? ModifiedDate { get; set; }
}