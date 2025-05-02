using System.ComponentModel.DataAnnotations;

namespace GestionTareasApi.Models;

public class UsuariosModel
{
    [Key]
    public int Id { get; set; }

    // NOMBRE DE USUARIO
    [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
    [StringLength(30, ErrorMessage = "El nombre de usuario no puede exceder los 30 caracteres.")]
    public string NombreUsuario { get; set; }

    // NOMBRE COMPLETO
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
    public string Nombre { get; set; }

    // EMAIL
    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
    [StringLength(100, ErrorMessage = "El correo electrónico no puede exceder los 100 caracteres.")]
    public string Email { get; set; }

    // CONTRASEÑA
    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [StringLength(100, ErrorMessage = "La contraseña no puede exceder los 100 caracteres.")]
    public string Contraseña { get; set; }

    // ROL
    [Required(ErrorMessage = "El rol es obligatorio.")]
    [StringLength(20, ErrorMessage = "El rol no puede exceder los 20 caracteres.")]
    public string Rol { get; set; } = "Estudiante";

    // ACTIVO
    public bool Activo { get; set; } = true;
}
