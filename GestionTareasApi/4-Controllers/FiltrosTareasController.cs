using GestionTareasApi.DTOs;
using GestionTareasApi.Servicios;
using GestionTareasApi.Funciones;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionTareasApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class FiltrosTareasController : ControllerBase
{
    // INYECTA EL SERVICIO DE TAREAS
    private readonly TareasService _servicio;

    // CONSTRUCTOR DEL CONTROLADOR
    public FiltrosTareasController(TareasService servicio)
    {
        _servicio = servicio;
    }

    #region FILTRAR POR ESTADO

    /// <summary>
    /// FILTRA LAS TAREAS POR ESTADO
    /// </summary>
    [HttpGet("por-estado")]
    public async Task<ActionResult<IEnumerable<TareaDTO>>> FiltrarPorEstado([FromQuery] string estado)
    {
        // OBTIENE TODAS LAS TAREAS
        var tareas = await _servicio.ObtenerTodasAsync();

        // APLICA FILTRO POR ESTADO
        var filtradas = tareas.Where(FiltrosTarea.PorEstado(estado)).ToList();

        return Ok(filtradas);
    }

    #endregion

    #region FILTRAR POR FECHA DE VENCIMIENTO

    /// <summary>
    /// FILTRA LAS TAREAS POR FECHA DE VENCIMIENTO
    /// </summary>
    [HttpGet("por-fecha")]
    public async Task<ActionResult<IEnumerable<TareaDTO>>> FiltrarPorFecha([FromQuery] DateTime fecha)
    {
        var tareas = await _servicio.ObtenerTodasAsync();
        var filtradas = tareas.Where(FiltrosTarea.PorFecha(fecha)).ToList();
        return Ok(filtradas);
    }

    #endregion

    #region FILTRAR POR ESTADO Y FECHA

    /// <summary>
    /// FILTRA LAS TAREAS POR ESTADO Y FECHA DE VENCIMIENTO
    /// </summary>
    [HttpGet("por-estado-y-fecha")]
    public async Task<ActionResult<IEnumerable<TareaDTO>>> FiltrarPorEstadoYFecha(
        [FromQuery] string estado,
        [FromQuery] DateTime fecha)
    {
        var tareas = await _servicio.ObtenerTodasAsync();
        var filtradas = tareas.Where(FiltrosTarea.PorEstadoYFecha(estado, fecha)).ToList();
        return Ok(filtradas);
    }

    #endregion

    #region FILTRAR POR PRIORIDAD

    /// <summary>
    /// FILTRA LAS TAREAS POR PRIORIDAD GENERAL
    /// </summary>
    [HttpGet("por-prioridad")]
    public async Task<ActionResult<IEnumerable<TareaDTO>>> FiltrarPorPrioridad([FromQuery] int prioridad)
    {
        var tareas = await _servicio.ObtenerTodasAsync();
        var filtradas = tareas.Where(FiltrosTarea.PorPrioridad(prioridad)).ToList();
        return Ok(filtradas);
    }

    #endregion

    #region FILTRAR POR USUARIO ASIGNADO

    /// <summary>
    /// FILTRA LAS TAREAS POR USUARIO ASIGNADO
    /// </summary>
    [HttpGet("por-asignado")]
    public async Task<ActionResult<IEnumerable<TareaDTO>>> FiltrarPorAsignadoA([FromQuery] string usuario)
    {
        var tareas = await _servicio.ObtenerTodasAsync();
        var filtradas = tareas.Where(FiltrosTarea.PorAsignado(usuario)).ToList();
        return Ok(filtradas);
    }

    #endregion

    #region FILTRAR POR CATEGORÍA

    /// <summary>
    /// FILTRA LAS TAREAS POR CATEGORÍA
    /// </summary>
    [HttpGet("por-categoria")]
    public async Task<ActionResult<IEnumerable<TareaDTO>>> FiltrarPorCategoria([FromQuery] string categoria)
    {
        var tareas = await _servicio.ObtenerTodasAsync();
        var filtradas = tareas.Where(FiltrosTarea.PorCategoria(categoria)).ToList();
        return Ok(filtradas);
    }

    #endregion

    #region FILTRAR POR ETIQUETA EN DATOSADICIONALES

    /// <summary>
    /// FILTRA LAS TAREAS POR ETIQUETA DENTRO DE DATOSADICIONALES
    /// </summary>
    [HttpGet("por-etiqueta")]
    public async Task<ActionResult<IEnumerable<TareaDTO>>> FiltrarPorEtiqueta([FromQuery] string etiqueta)
    {
        var tareas = await _servicio.ObtenerTodasAsync();
        var filtradas = tareas.Where(FiltrosTarea.PorEtiqueta(etiqueta)).ToList();
        return Ok(filtradas);
    }

    #endregion

    #region FILTRAR POR PRIORIDAD EN DATOSADICIONALES

    /// <summary>
    /// FILTRA LAS TAREAS POR PRIORIDAD EN DATOSADICIONALES
    /// </summary>
    [HttpGet("por-prioridad-datos")]
    public async Task<ActionResult<IEnumerable<TareaDTO>>> FiltrarPorPrioridadEnDatos([FromQuery] int valor)
    {
        var tareas = await _servicio.ObtenerTodasAsync();
        var filtradas = tareas.Where(FiltrosTarea.PorPrioridadEnDatos(valor)).ToList();
        return Ok(filtradas);
    }

    #endregion
}
