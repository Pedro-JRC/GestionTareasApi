using GestionTareasApi.DTOs;
using GestionTareasApi.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionTareasApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TareasController : ControllerBase
{
    private readonly TareasService _servicio;

    public TareasController(TareasService servicio)
    {
        _servicio = servicio;
    }

    #region OBTENER TODAS LAS TAREAS

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TareaDTO>>> ObtenerTodas()
    {
        var tareas = await _servicio.ObtenerTodasAsync();
        return Ok(tareas);
    }

    #endregion

    #region OBTENER TAREA POR ID

    [HttpGet("{id}")]
    public async Task<ActionResult<TareaDTO>> ObtenerPorId(int id)
    {
        var tarea = await _servicio.ObtenerPorIdAsync(id);
        if (tarea == null)
            return NotFound(new { mensaje = "Tarea no encontrada." });

        return Ok(tarea);
    }

    #endregion

    #region CREAR NUEVA TAREA

    [HttpPost]
    public async Task<ActionResult> Crear([FromBody] CrearTareaDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var (exitoso, mensaje, nuevaTarea) = await _servicio.CrearAsync(dto);

        if (!exitoso)
            return BadRequest(new { mensaje });

        return CreatedAtAction(nameof(ObtenerPorId), new { id = nuevaTarea!.Id }, nuevaTarea);
    }

    #endregion

    #region ACTUALIZAR TAREA EXISTENTE

    [HttpPut("{id}")]
    public async Task<ActionResult> Actualizar(int id, [FromBody] ActualizarTareaDTO dto)
    {
        if (id != dto.Id)
            return BadRequest(new { mensaje = "El ID no coincide con el cuerpo de la solicitud." });

        var (exitoso, mensaje) = await _servicio.ActualizarAsync(dto);
        if (!exitoso)
            return NotFound(new { mensaje });

        return Ok(new { mensaje });
    }

    #endregion

    #region ELIMINAR TAREA

    [HttpDelete("{id}")]
    public async Task<ActionResult> Eliminar(int id)
    {
        var eliminado = await _servicio.EliminarAsync(id);
        if (!eliminado)
            return NotFound(new { mensaje = "Tarea no encontrada para eliminar." });

        return Ok(new { mensaje = "Tarea eliminada correctamente." });
    }

    #endregion
}
