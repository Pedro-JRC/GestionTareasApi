using GestionTareasApi.Models;
using GestionTareasApi.Data;
using GestionTareasApi.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

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

        return (true, "Tarea creada correctamente.", MapearADTO(entidad));
    }

    #endregion

    #region ACTUALIZAR UNA TAREA EXISTENTE

    public async Task<(bool Exitoso, string Mensaje)> ActualizarAsync(ActualizarTareaDTO dto)
    {
        var tarea = await _context.Tareas.FindAsync(dto.Id);
        if (tarea == null)
            return (false, "Tarea no encontrada.");

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
        return (true, "Tarea actualizada correctamente.");
    }

    #endregion

    #region ELIMINAR UNA TAREA

    public async Task<bool> EliminarAsync(int id)
    {
        var tarea = await _context.Tareas.FindAsync(id);
        if (tarea == null) return false;

        _context.Tareas.Remove(tarea);
        await _context.SaveChangesAsync();
        return true;
    }

    #endregion

    #region MAPEO A DTO

    private TareaDTO MapearADTO(TareaGeneral tarea)
    {
        return new TareaDTO
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
    }

    #endregion
}
