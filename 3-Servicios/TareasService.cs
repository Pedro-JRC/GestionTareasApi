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
using GestionTareasApi._3_Servicios;

namespace GestionTareasApi.Servicios;

public class TareasService
{
    private readonly AppDbContext _context;
    private readonly ColaTareasRxService _cola;

    public TareasService(AppDbContext context, ColaTareasRxService cola)
    {
        _context = context;
        _cola = cola;
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

    /// <summary>
    /// AGREGA UNA NUEVA TAREA A LA COLA PARA SER PROCESADA DE FORMA SECUENCIAL (FIFO).
    /// EL MÉTODO NO DEVUELVE INMEDIATAMENTE LA TAREA, SINO UN RESULTADO DE ENCOLADO.
    /// </summary>
    public Task<(bool Exitoso, string? Mensaje, TareaDTO? Resultado)> CrearAsync(CrearTareaDTO dto)
    {
        // RETORNA UNA RESPUESTA INMEDIATA CONFIRMANDO QUE LA TAREA FUE ENCOLADA
        _cola.Agregar(async () =>
        {
            ValidadorTareaDelegate validador = ValidacionesTareaService.ValidarDatosBasicos;
            var (valido, mensajeValidacion) = validador(dto);
            if (!valido) return;

            if (!string.IsNullOrWhiteSpace(dto.AsignadoA))
            {
                var existeUsuario = await _context.Usuarios
                    .AnyAsync(u => u.NombreUsuario == dto.AsignadoA && u.Activo);

                if (!existeUsuario) return;
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
        });

        // RETORNA UNA RESPUESTA DE CONFIRMACIÓN INMEDIATA (LA EJECUCIÓN OCURRIRÁ LUEGO)
        return Task.FromResult<(bool, string?, TareaDTO?)>(
            (true, "Tarea agregada a la cola para ser procesada.", null));
    }

    #endregion


    #region CREAR TAREA PREDEFINIDA DESDE FÁBRICA

    /// <summary>
    /// CREA UNA TAREA USANDO UNA CONFIGURACIÓN PREDEFINIDA SEGÚN SU TIPO.
    /// SE PUEDEN CREAR TAREAS DE TIPO: ALTA, URGENTE O DOCUMENTACION.
    /// LA EJECUCIÓN ES ENCOLADA Y SE PROCESA DE FORMA SECUENCIAL.
    /// </summary>
    public Task<(bool Exitoso, string? Mensaje, TareaDTO? Resultado)> CrearDesdeFactoryAsync(
        TipoTareaPredefinida tipo, string titulo, string descripcion, string asignadoA)
    {
        _cola.Agregar(async () =>
        {
            TareaGeneral? entidad = tipo switch
            {
                TipoTareaPredefinida.Alta => TareaFactory.CrearTareaAltaPrioridad(titulo, descripcion, asignadoA),
                TipoTareaPredefinida.Urgente => TareaFactory.CrearTareaUrgente(titulo, descripcion, asignadoA),
                TipoTareaPredefinida.Documentacion => TareaFactory.CrearTareaDocumentacion(titulo, descripcion, asignadoA),
                _ => null
            };

            if (entidad == null) return;

            if (!string.IsNullOrWhiteSpace(asignadoA))
            {
                var existeUsuario = await _context.Usuarios
                    .AnyAsync(u => u.NombreUsuario == asignadoA && u.Activo);

                if (!existeUsuario) return;
            }

            _context.Tareas.Add(entidad);
            await _context.SaveChangesAsync();

            var tareaDTO = MapearADTO(entidad);
            EventosTarea.RegistrarEvento(tareaDTO);
        });

        // SE RETORNA CONFIRMACIÓN DE QUE LA TAREA FUE ENCOLADA
        return Task.FromResult<(bool, string?, TareaDTO?)>(
            (true, "Tarea predefinida agregada a la cola para ser procesada.", null));
    }

    #endregion


    #region ACTUALIZAR UNA TAREA EXISTENTE

    /// <summary>
    /// ACTUALIZA UNA TAREA EXISTENTE USANDO LA COLA PARA GARANTIZAR ORDEN Y SEGURIDAD.
    /// LA ACTUALIZACIÓN SE EJECUTA DE FORMA SECUENCIAL EN SEGUNDO PLANO.
    /// </summary>
    public Task<(bool Exitoso, string Mensaje)> ActualizarAsync(ActualizarTareaDTO dto)
    {
        _cola.Agregar(async () =>
        {
            var tarea = await _context.Tareas.FindAsync(dto.Id);
            if (tarea == null) return;

            ValidadorTareaActualizadaDelegate validador = ValidacionesTareaService.ValidarDatosActualizados;
            var (valido, mensajeValidacion) = validador(dto);
            if (!valido) return;

            if (!string.IsNullOrWhiteSpace(dto.AsignadoA))
            {
                var existeUsuario = await _context.Usuarios
                    .AnyAsync(u => u.NombreUsuario == dto.AsignadoA && u.Activo);

                if (!existeUsuario) return;
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
        });

        // RESPUESTA INMEDIATA DE ENCOLADO, NO DEL RESULTADO FINAL
        return Task.FromResult<(bool, string)>((true, "Tarea encolada para ser actualizada."));
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
