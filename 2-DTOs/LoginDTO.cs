using System.ComponentModel.DataAnnotations;

namespace GestionTareasApi.DTOs;

public class LoginDTO
{
    [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
    public string NombreUsuario { get; set; }

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    public string Contraseña { get; set; }
}
