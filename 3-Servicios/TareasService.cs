using GestionTareasApi.Models;
using GestionTareasApi.Data;
using GestionTareasApi.DTOs;
using GestionTareasApi.Eventos;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using static GestionTareasApi.Delegados.DelegadosTarea;
using GestionTareasApi.Funciones;
using GestionTareasApi.Fabricas;
using GestionTareasApi.Enums;

namespace GestionTareasApi.Servicios;

public class TareasService
{
    private readonly AppDbContext _context;

    public TareasService(AppDbContext context)
    {
        _context = context;
    }

    #region OBTENER TODAS LAS TAREAS

    public async Task<List<TareaDTO>> ObtenerTodasAsync()
    {
        var tareas = await _context.Tareas.ToListAsync();
        return tareas.Select(MapearADTO).ToList();
    }

    #endregion

    #region OBTENER UNA TAREA POR ID

    public async Task<TareaDTO?> ObtenerPorIdAsync(int id)
    {
        var tarea = await _context.Tareas.FindAsync(id);
        return tarea == null ? null : MapearADTO(tarea);
    }

    #endregion

    #region CREAR UNA NUEVA TAREA

    public async Task<(bool Exitoso, string? Mensaje, TareaDTO? Resultado)> CrearAsync(CrearTareaDTO dto)
    {
        ValidadorTareaDelegate validador = ValidacionesTareaService.ValidarDatosBasicos;
        var (valido, mensajeValidacion) = validador(dto);

        if (!valido)
            return (false, mensajeValidacion, null);

        if (!string.IsNullOrWhiteSpace(dto.AsignadoA))
        {
            var existeUsuario = await _context.Usuarios
                .AnyAsync(u => u.NombreUsuario == dto.AsignadoA && u.Activo);

            if (!existeUsuario)
                return (false, $"El usuario '{dto.AsignadoA}' no existe o está inactivo.", null);
        }

        var entidad = new TareaGeneral
        {
            Titulo = dto.Titulo,
            Descripcion = dto.Descripcion,
            FechaVencimiento = dto.FechaVencimiento,
            Estado = dto.Estado,
            DatosAdicionales = dto.DatosAdicionales != null
                ? JsonSerializer.Serialize(dto.DatosAdicionales)
                : null,
            Prioridad = dto.Prioridad,
            Categoria = dto.Categoria,
            AsignadoA = dto.AsignadoA
        };

        _context.Tareas.Add(entidad);
        await _context.SaveChangesAsync();

        var tareaDTO = MapearADTO(entidad);
        EventosTarea.RegistrarEvento(tareaDTO);

        var mensajeEvento = EventosTarea.GenerarMensaje(tareaDTO);
        return (true, mensajeEvento, tareaDTO);
    }

    #endregion

    #region CREAR TAREA PREDEFINIDA DESDE FÁBRICA

    /// <summary>
    /// CREA UNA TAREA USANDO UNA CONFIGURACIÓN PREDEFINIDA SEGÚN SU TIPO.
    /// SE PUEDEN CREAR TAREAS DE TIPO: ALTA, URGENTE O DOCUMENTACION.
    /// </summary>
    public async Task<(bool Exitoso, string? Mensaje, TareaDTO? Resultado)> CrearDesdeFactoryAsync(
        TipoTareaPredefinida tipo, string titulo, string descripcion, string asignadoA)
    {
        TareaGeneral? entidad = tipo switch
        {
            TipoTareaPredefinida.Alta => TareaFactory.CrearTareaAltaPrioridad(titulo, descripcion, asignadoA),
            TipoTareaPredefinida.Urgente => TareaFactory.CrearTareaUrgente(titulo, descripcion, asignadoA),
            TipoTareaPredefinida.Documentacion => TareaFactory.CrearTareaDocumentacion(titulo, descripcion, asignadoA),
            _ => null
        };

        // VALIDACIÓN DEL USUARIO ASIGNADO (SI EXISTE)
        if (!string.IsNullOrWhiteSpace(asignadoA))
        {
            var existeUsuario = await _context.Usuarios
                .AnyAsync(u => u.NombreUsuario == asignadoA && u.Activo);

            if (!existeUsuario)
                return (false, $"El usuario '{asignadoA}' no existe o está inactivo.", null);
        }

        _context.Tareas.Add(entidad!);
        await _context.SaveChangesAsync();

        var tareaDTO = MapearADTO(entidad!);
        EventosTarea.RegistrarEvento(tareaDTO);

        var mensaje = EventosTarea.GenerarMensaje(tareaDTO);
        return (true, mensaje, tareaDTO);
    }

    #endregion



    #region ACTUALIZAR UNA TAREA EXISTENTE

    public async Task<(bool Exitoso, string Mensaje)> ActualizarAsync(ActualizarTareaDTO dto)
    {
        var tarea = await _context.Tareas.FindAsync(dto.Id);
        if (tarea == null)
            return (false, "Tarea no encontrada.");

        ValidadorTareaActualizadaDelegate validador = ValidacionesTareaService.ValidarDatosActualizados;
        var (valido, mensajeValidacion) = validador(dto);

        if (!valido)
            return (false, mensajeValidacion!);

        if (!string.IsNullOrWhiteSpace(dto.AsignadoA))
        {
            var existeUsuario = await _context.Usuarios
                .AnyAsync(u => u.NombreUsuario == dto.AsignadoA && u.Activo);

            if (!existeUsuario)
                return (false, $"El usuario '{dto.AsignadoA}' no existe o está inactivo.");
        }

        tarea.Titulo = dto.Titulo;
        tarea.Descripcion = dto.Descripcion;
        tarea.FechaVencimiento = dto.FechaVencimiento;
        tarea.Estado = dto.Estado;
        tarea.DatosAdicionales = dto.DatosAdicionales != null
            ? JsonSerializer.Serialize(dto.DatosAdicionales)
            : null;
        tarea.FechaCompletado = dto.FechaCompletado;
        tarea.Prioridad = dto.Prioridad;
        tarea.Categoria = dto.Categoria;
        tarea.AsignadoA = dto.AsignadoA;
        tarea.EstaActiva = dto.EstaActiva;

        await _context.SaveChangesAsync();
        EventosTarea.RegistrarEvento(MapearADTO(tarea));

        return (true, "Tarea actualizada correctamente.");
    }

    #endregion



    #region ELIMINAR UNA TAREA

    public async Task<bool> EliminarAsync(int id)
    {
        var tarea = await _context.Tareas.FindAsync(id);
        if (tarea == null) return false;

        var dto = MapearADTO(tarea);
        _context.Tareas.Remove(tarea);
        await _context.SaveChangesAsync();

        EventosTarea.RegistrarEvento(dto);
        return true;
    }

    #endregion

    #region MAPEO A DTO

    private TareaDTO MapearADTO(TareaGeneral tarea)
    {
        var dto = new TareaDTO
        {
            Id = tarea.Id,
            Titulo = tarea.Titulo,
            Descripcion = tarea.Descripcion,
            FechaVencimiento = tarea.FechaVencimiento,
            Estado = tarea.Estado,
            DatosAdicionales = tarea.DatosAdicionales,
            FechaCreacion = tarea.FechaCreacion,
            FechaCompletado = tarea.FechaCompletado,
            Prioridad = tarea.Prioridad,
            Categoria = tarea.Categoria,
            AsignadoA = tarea.AsignadoA,
            EstaActiva = tarea.EstaActiva
        };

        dto.DiasRestantes = FuncionesTarea.CalcularDiasRestantes(dto);
        return dto;
    }


    #endregion
}
