using GestionTareasApi.DTOs;
using GestionTareasApi.Enums;
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
    /// LA TAREA SE ENCOLA Y SE PROCESARÁ DE FORMA SECUENCIAL.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult> Crear([FromBody] CrearTareaDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var (exitoso, mensaje, _) = await _servicio.CrearAsync(dto);

        if (!exitoso)
            return BadRequest(new { mensaje });

        return Ok(new { mensaje });
    }

    #endregion

    #region CREAR TAREA CON CONFIGURACIÓN PREDETERMINADA

    /// <summary>
    /// CREA UNA TAREA PRECONFIGURADA USANDO EL PATRÓN FÁBRICA.
    /// SELECCIONE EL TIPO DE TAREA ENTRE: 'Alta', 'Urgente' O 'Documentacion'.
    /// LA TAREA SE ENCOLA Y SE PROCESA LUEGO.
    /// </summary>
    [HttpPost("crear-desde-fabrica")]
    public async Task<IActionResult> CrearDesdeFactory(
        [FromQuery] TipoTareaPredefinida tipo,
        [FromBody] CrearTareaPredefinidaDTO dto)
    {
        var (exitoso, mensaje, _) = await _servicio.CrearDesdeFactoryAsync(
            tipo, dto.Titulo, dto.Descripcion, dto.AsignadoA
        );

        if (!exitoso)
            return BadRequest(new { mensaje });

        return Ok(new { mensaje });
    }

    #endregion

    #region ACTUALIZAR TAREA EXISTENTE

    /// <summary>
    /// ACTUALIZA UNA TAREA EXISTENTE POR SU ID.
    /// LA ACTUALIZACIÓN SE ENCOLA PARA EJECUTARSE DE FORMA SECUENCIAL.
    /// </summary>
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

    #region OBTENER PORCENTAJE DE TAREAS COMPLETADAS

    /// <summary>
    /// OBTIENE EL PORCENTAJE DE TAREAS COMPLETADAS USANDO MEMORIZACIÓN.
    /// </summary>
    [HttpGet("porcentaje-completadas")]
    public async Task<ActionResult> ObtenerPorcentajeTareasCompletadas()
    {
        var tareas = await _servicio.ObtenerTodasAsync();
        var porcentaje = _servicio.CalcularPorcentajeCompletadas(tareas);
        return Ok(new { porcentaje });
    }

    #endregion

}
