using System.ComponentModel.DataAnnotations;

namespace GestionTareasApi.Models;

// MODELO GENÉRICO PARA TAREAS CON DATOS ADICIONALES DINÁMICOS
public class TareaModel<T>
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "El título es obligatorio.")]
    [StringLength(100, ErrorMessage = "El título no puede exceder los 100 caracteres.")]
    public string Titulo { get; set; }

    [Required(ErrorMessage = "La descripción es obligatoria.")]
    [MinLength(5, ErrorMessage = "La descripción debe tener al menos 5 caracteres.")]
    public string Descripcion { get; set; } = string.Empty;

    [Required(ErrorMessage = "La fecha de vencimiento es obligatoria.")]
    public DateTime FechaVencimiento { get; set; }

    [Required(ErrorMessage = "El estado es obligatorio.")]
    public string Estado { get; set; } = "Pendiente";

    // DATO ADICIONAL GENÉRICO QUE VARÍA SEGÚN EL TIPO DE TAREA
    public T? DatosAdicionales { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    [DataType(DataType.DateTime)]
    public DateTime? FechaCompletado { get; set; }

    [Range(1, 5, ErrorMessage = "La prioridad debe estar entre 1 y 5.")]
    public int? Prioridad { get; set; }

    [StringLength(50)]
    public string? Categoria { get; set; }

    [StringLength(100)]
    public string? AsignadoA { get; set; }

    public bool EstaActiva { get; set; } = true;
}
