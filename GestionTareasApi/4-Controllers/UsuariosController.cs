using GestionTareasApi.DTOs;
using GestionTareasApi.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionTareasApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    // INYECTA EL SERVICIO DE USUARIOS
    private readonly UsuariosService _servicio;

    // CONSTRUCTOR DEL CONTROLADOR
    public UsuariosController(UsuariosService servicio)
    {
        _servicio = servicio;
    }

    #region REGISTRAR USUARIO

    /// <summary>
    /// REGISTRA UN NUEVO USUARIO
    /// </summary>
    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult> Registrar([FromBody] CrearUsuarioDTO dto)
    {
        // VERIFICA QUE EL MODELO SEA VÁLIDO
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // LLAMA AL SERVICIO PARA CREAR EL USUARIO
        var (exitoso, mensaje) = await _servicio.CrearAsync(dto);

        // SI HUBO ERROR AL CREAR, DEVUELVE BADREQUEST
        if (!exitoso)
            return BadRequest(new { mensaje });

        // RETORNA MENSAJE DE ÉXITO
        return Ok(new { mensaje });
    }

    #endregion

    #region OBTENER TODOS LOS USUARIOS

    /// <summary>
    /// OBTIENE TODOS LOS USUARIOS REGISTRADOS
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UsuariosDTO>>> ObtenerTodos()
    {
        var usuarios = await _servicio.ObtenerTodosAsync();
        return Ok(usuarios);
    }

    #endregion

    #region OBTENER USUARIO POR ID

    /// <summary>
    /// OBTIENE UN USUARIO POR SU ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<UsuariosDTO>> ObtenerPorId(int id)
    {
        var usuario = await _servicio.ObtenerPorIdAsync(id);

        // RETORNA NOTFOUND SI NO EXISTE
        if (usuario == null)
            return NotFound(new { mensaje = "Usuario no encontrado." });

        return Ok(usuario);
    }

    #endregion

    #region ACTUALIZAR USUARIO

    /// <summary>
    /// ACTUALIZA UN USUARIO EXISTENTE
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult> Actualizar(int id, [FromBody] ActualizarUsuarioDTO dto)
    {
        // VERIFICA QUE EL ID DE LA RUTA COINCIDA CON EL DEL CUERPO
        if (id != dto.Id)
            return BadRequest(new { mensaje = "El ID del cuerpo no coincide con el de la ruta." });

        // VALIDA EL MODELO
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var (exitoso, mensaje) = await _servicio.ActualizarAsync(dto);

        // SI NO SE ENCONTRÓ EL USUARIO A ACTUALIZAR
        if (!exitoso)
            return NotFound(new { mensaje });

        return Ok(new { mensaje });
    }

    #endregion

    #region ELIMINAR USUARIO

    /// <summary>
    /// ELIMINA UN USUARIO POR SU ID
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Eliminar(int id)
    {
        var (exitoso, mensaje) = await _servicio.EliminarAsync(id);

        // SI NO EXISTE EL USUARIO
        if (!exitoso)
            return NotFound(new { mensaje });

        return Ok(new { mensaje });
    }

    #endregion
}
