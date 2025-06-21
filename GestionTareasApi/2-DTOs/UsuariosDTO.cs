using System.ComponentModel.DataAnnotations;

namespace GestionTareasApi.DTOs;

#region DTO PARA CREAR USUARIO

public class CrearUsuarioDTO
{
    [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
    [StringLength(30, ErrorMessage = "El nombre de usuario no puede exceder los 30 caracteres.")]
    public string NombreUsuario { get; set; }

    [Required(ErrorMessage = "El nombre completo es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
    public string Nombre { get; set; }

    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
    [StringLength(100, ErrorMessage = "El correo electrónico no puede exceder los 100 caracteres.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [StringLength(100, ErrorMessage = "La contraseña no puede exceder los 100 caracteres.")]
    public string Contraseña { get; set; }

    [Required(ErrorMessage = "Debe confirmar la contraseña.")]
    [Compare("Contraseña", ErrorMessage = "Las contraseñas no coinciden.")]
    public string ConfirmarContraseña { get; set; }

    [StringLength(20, ErrorMessage = "El rol no puede exceder los 20 caracteres.")]
    public string Rol { get; set; } = "Estudiante";
}

#endregion

#region DTO PARA ACTUALIZAR USUARIO

public class ActualizarUsuarioDTO
{
    [Required(ErrorMessage = "El ID del usuario es obligatorio.")]
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
    [StringLength(30)]
    public string NombreUsuario { get; set; }

    [Required(ErrorMessage = "El nombre completo es obligatorio.")]
    [StringLength(100)]
    public string Nombre { get; set; }

    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; }

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [StringLength(100)]
    public string Contraseña { get; set; }

    [Required(ErrorMessage = "Debe confirmar la contraseña.")]
    [Compare("Contraseña", ErrorMessage = "Las contraseñas no coinciden.")]
    public string ConfirmarContraseña { get; set; }

    [StringLength(20)]
    public string Rol { get; set; }

    public bool Activo { get; set; }
}

#endregion

#region DTO PARA CONSULTAR USUARIO

public class UsuariosDTO
{
    public int Id { get; set; }
    public string NombreUsuario { get; set; }
    public string Nombre { get; set; }
    public string Email { get; set; }
    public string Rol { get; set; }
    public bool Activo { get; set; }
}

#endregion
