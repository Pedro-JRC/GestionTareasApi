using GestionTareasApi.DTOs;
using GestionTareasApi.Servicios;
using GestionTareasApi.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionTareasApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class FiltrosTareasController : ControllerBase
{
    private readonly TareasService _servicio;

    public FiltrosTareasController(TareasService servicio)
    {
        _servicio = servicio;
    }

    #region FILTRAR POR ESTADO

    [HttpGet("por-estado")]
    public async Task<ActionResult<IEnumerable<TareaDTO>>> FiltrarPorEstado([FromQuery] string estado)
    {
        var tareas = await _servicio.ObtenerTodasAsync();
        var filtradas = tareas.Where(t => t.Estado.ToLower() == estado.ToLower()).ToList();
        return Ok(filtradas);
    }

    #endregion

    #region FILTRAR POR FECHA DE VENCIMIENTO

    [HttpGet("por-fecha")]
    public async Task<ActionResult<IEnumerable<TareaDTO>>> FiltrarPorFecha([FromQuery] DateTime fecha)
    {
        var tareas = await _servicio.ObtenerTodasAsync();
        var filtradas = tareas.Where(t => t.FechaVencimiento.Date == fecha.Date).ToList();
        return Ok(filtradas);
    }

    #endregion

    #region FILTRAR POR ESTADO Y FECHA

    [HttpGet("por-estado-y-fecha")]
    public async Task<ActionResult<IEnumerable<TareaDTO>>> FiltrarPorEstadoYFecha(
        [FromQuery] string estado,
        [FromQuery] DateTime fecha)
    {
        var tareas = await _servicio.ObtenerTodasAsync();
        var filtradas = tareas.Where(t =>
            t.Estado.ToLower() == estado.ToLower() &&
            t.FechaVencimiento.Date == fecha.Date
        ).ToList();

        return Ok(filtradas);
    }

    #endregion

    #region FILTRAR POR PRIORIDAD

    [HttpGet("por-prioridad")]
    public async Task<ActionResult<IEnumerable<TareaDTO>>> FiltrarPorPrioridad([FromQuery] int prioridad)
    {
        var tareas = await _servicio.ObtenerTodasAsync();
        var filtradas = tareas.Where(t => t.Prioridad == prioridad).ToList();
        return Ok(filtradas);
    }

    #endregion

    #region FILTRAR POR USUARIO ASIGNADO

    [HttpGet("por-asignado")]
    public async Task<ActionResult<IEnumerable<TareaDTO>>> FiltrarPorAsignadoA([FromQuery] string usuario)
    {
        var tareas = await _servicio.ObtenerTodasAsync();
        var filtradas = tareas.Where(t =>
            !string.IsNullOrEmpty(t.AsignadoA) &&
            t.AsignadoA.ToLower().Contains(usuario.ToLower())
        ).ToList();

        return Ok(filtradas);
    }

    #endregion

    #region FILTRAR POR CATEGORÍA

    [HttpGet("por-categoria")]
    public async Task<ActionResult<IEnumerable<TareaDTO>>> FiltrarPorCategoria([FromQuery] string categoria)
    {
        var tareas = await _servicio.ObtenerTodasAsync();
        var filtradas = tareas.Where(t =>
            !string.IsNullOrEmpty(t.Categoria) &&
            t.Categoria.ToLower().Contains(categoria.ToLower())
        ).ToList();

        return Ok(filtradas);
    }

    #endregion

    #region FILTRAR POR ETIQUETA EN DATOSADICIONALES

    [HttpGet("por-etiqueta")]
    public async Task<ActionResult<IEnumerable<TareaDTO>>> FiltrarPorEtiqueta([FromQuery] string etiqueta)
    {
        var tareas = await _servicio.ObtenerTodasAsync();

        var filtradas = tareas.Where(t =>
        {
            var lista = DatosAdicionalesHelper.ComoListaDeTexto(t.DatosAdicionales);
            return lista != null && lista.Any(e => e.Equals(etiqueta, StringComparison.OrdinalIgnoreCase));
        }).ToList();

        return Ok(filtradas);
    }

    #endregion

    #region FILTRAR POR PRIORIDAD EN DATOSADICIONALES

    [HttpGet("por-prioridad-datos")]
    public async Task<ActionResult<IEnumerable<TareaDTO>>> FiltrarPorPrioridadEnDatos([FromQuery] int valor)
    {
        var tareas = await _servicio.ObtenerTodasAsync();

        var filtradas = tareas.Where(t =>
        {
            var numero = DatosAdicionalesHelper.ComoEntero(t.DatosAdicionales);
            return numero.HasValue && numero.Value == valor;
        }).ToList();

        return Ok(filtradas);
    }

    #endregion
}
