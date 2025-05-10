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
    // INYECTA EL SERVICIO DE TAREAS
    private readonly TareasService _servicio;

    // CONSTRUCTOR DEL CONTROLADOR
    public TareasController(TareasService servicio)
    {
        _servicio = servicio;
    }

    #region OBTENER TODAS LAS TAREAS

    /// <summary>
    /// OBTIENE TODAS LAS TAREAS REGISTRADAS
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TareaDTO>>> ObtenerTodas()
    {
        var tareas = await _servicio.ObtenerTodasAsync();
        return Ok(tareas);
    }

    #endregion

    #region OBTENER TAREA POR ID

    /// <summary>
    /// OBTIENE UNA TAREA ESPECÍFICA POR SU ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<TareaDTO>> ObtenerPorId(int id)
    {
        var tarea = await _servicio.ObtenerPorIdAsync(id);

        // VALIDACIÓN SI NO EXISTE LA TAREA
        if (tarea == null)
            return NotFound(new { mensaje = "Tarea no encontrada." });

        return Ok(tarea);
    }

    #endregion

    #region CREAR NUEVA TAREA

    /// <summary>
    /// CREA UNA NUEVA TAREA CON LOS DATOS RECIBIDOS
    /// </summary>
    [HttpPost]
    public async Task<ActionResult> Crear([FromBody] CrearTareaDTO dto)
    {
        // VALIDACIÓN DE MODELO
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // LLAMA AL SERVICIO PARA CREAR LA TAREA
        var (exitoso, mensaje, nuevaTarea) = await _servicio.CrearAsync(dto);

        // SI HUBO ERROR AL CREAR, RETORNA BADREQUEST
        if (!exitoso)
            return BadRequest(new { mensaje });

        // RETORNA LA NUEVA TAREA CREADA
        return Ok(new
        {
            mensaje,
            tarea = nuevaTarea
        });
    }

    #endregion

    #region ACTUALIZAR TAREA EXISTENTE

    /// <summary>
    /// ACTUALIZA UNA TAREA EXISTENTE POR SU ID
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult> Actualizar(int id, [FromBody] ActualizarTareaDTO dto)
    {
        // VERIFICA SI EL ID DE LA URL COINCIDE CON EL DEL CUERPO
        if (id != dto.Id)
            return BadRequest(new { mensaje = "El ID no coincide con el cuerpo de la solicitud." });

        // LLAMA AL SERVICIO PARA ACTUALIZAR
        var (exitoso, mensaje) = await _servicio.ActualizarAsync(dto);

        // SI NO EXISTE LA TAREA
        if (!exitoso)
            return NotFound(new { mensaje });

        return Ok(new { mensaje });
    }

    #endregion

    #region ELIMINAR TAREA

    /// <summary>
    /// ELIMINA UNA TAREA POR SU ID
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Eliminar(int id)
    {
        var eliminado = await _servicio.EliminarAsync(id);

        // SI LA TAREA NO EXISTE
        if (!eliminado)
            return NotFound(new { mensaje = "Tarea no encontrada para eliminar." });

        return Ok(new { mensaje = "Tarea eliminada correctamente." });
    }

    #endregion
}
