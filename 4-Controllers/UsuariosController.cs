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
    private readonly UsuariosService _servicio;

    public UsuariosController(UsuariosService servicio)
    {
        _servicio = servicio;
    }

    #region REGISTRAR USUARIO
    
    [HttpPost]
    public async Task<ActionResult> Registrar([FromBody] CrearUsuarioDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var (exitoso, mensaje) = await _servicio.CrearAsync(dto);

        if (!exitoso)
            return BadRequest(new { mensaje });

        return Ok(new { mensaje });
    }

    #endregion

    #region OBTENER TODOS LOS USUARIOS

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UsuariosDTO>>> ObtenerTodos()
    {
        var usuarios = await _servicio.ObtenerTodosAsync();
        return Ok(usuarios);
    }

    #endregion

    #region OBTENER USUARIO POR ID

    [HttpGet("{id}")]
    public async Task<ActionResult<UsuariosDTO>> ObtenerPorId(int id)
    {
        var usuario = await _servicio.ObtenerPorIdAsync(id);
        if (usuario == null)
            return NotFound(new { mensaje = "Usuario no encontrado." });

        return Ok(usuario);
    }

    #endregion

    #region ACTUALIZAR USUARIO

    [HttpPut("{id}")]
    public async Task<ActionResult> Actualizar(int id, [FromBody] ActualizarUsuarioDTO dto)
    {
        if (id != dto.Id)
            return BadRequest(new { mensaje = "El ID del cuerpo no coincide con el de la ruta." });

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var (exitoso, mensaje) = await _servicio.ActualizarAsync(dto);
        if (!exitoso)
            return NotFound(new { mensaje });

        return Ok(new { mensaje });
    }

    #endregion

    #region ELIMINAR USUARIO

    [HttpDelete("{id}")]
    public async Task<ActionResult> Eliminar(int id)
    {
        var (exitoso, mensaje) = await _servicio.EliminarAsync(id);
        if (!exitoso)
            return NotFound(new { mensaje });

        return Ok(new { mensaje });
    }

    #endregion
}
