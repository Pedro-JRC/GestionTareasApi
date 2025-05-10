namespace GestionTareasApi.DTOs;

#region DTO PARA CREAR UNA TAREA

public class CrearTareaDTO
{
    // TÍTULO DE LA TAREA
    public string Titulo { get; set; }

    // DESCRIPCIÓN DE LA TAREA
    public string Descripcion { get; set; }

    // FECHA DE VENCIMIENTO
    public DateTime FechaVencimiento { get; set; }

    // ESTADO (Ej: Pendiente, Completada)
    public string Estado { get; set; }

    // CAMPO ADICIONAL (lista, número, texto...)
    public object? DatosAdicionales { get; set; }

    // PRIORIDAD (1–5)
    public int? Prioridad { get; set; }

    // CATEGORÍA DE LA TAREA
    public string? Categoria { get; set; }

    // USUARIO ASIGNADO
    public string? AsignadoA { get; set; }
}

#endregion

#region DTO PARA ACTUALIZAR UNA TAREA

public class ActualizarTareaDTO
{
    public int Id { get; set; }

    // TÍTULO DE LA TAREA
    public string Titulo { get; set; }

    // DESCRIPCIÓN DE LA TAREA
    public string Descripcion { get; set; }

    // FECHA DE VENCIMIENTO
    public DateTime FechaVencimiento { get; set; }

    // ESTADO (Ej: Pendiente, Completada)
    public string Estado { get; set; }

    // CAMPO ADICIONAL (texto, lista, número...) 
    public object? DatosAdicionales { get; set; }

    // FECHA DE COMPLETADO
    public DateTime? FechaCompletado { get; set; }

    // PRIORIDAD (1–5)
    public int? Prioridad { get; set; }

    // CATEGORÍA
    public string? Categoria { get; set; }

    // USUARIO ASIGNADO
    public string? AsignadoA { get; set; }

    // INDICADOR DE SI LA TAREA ESTÁ ACTIVA
    public bool EstaActiva { get; set; }
}

#endregion

#region DTO DE RESPUESTA

public class TareaDTO
{
    public int Id { get; set; }

    // TÍTULO DE LA TAREA
    public string Titulo { get; set; }

    // DESCRIPCIÓN DE LA TAREA
    public string Descripcion { get; set; }

    // FECHA DE VENCIMIENTO
    public DateTime FechaVencimiento { get; set; }

    // ESTADO
    public string Estado { get; set; }

    // DATO ADICIONAL (GUARDADO COMO STRING SERIALIZADO)
    public string? DatosAdicionales { get; set; }

    // FECHA DE CREACIÓN
    public DateTime FechaCreacion { get; set; }

    // FECHA DE COMPLETADO
    public DateTime? FechaCompletado { get; set; }

    // PRIORIDAD
    public int? Prioridad { get; set; }

    // CATEGORÍA
    public string? Categoria { get; set; }

    // USUARIO ASIGNADO
    public string? AsignadoA { get; set; }

    // ESTADO ACTIVO
    public bool EstaActiva { get; set; }


    // DIAS RESTANTES (calculados con FUNC)
    public int DiasRestantes { get; set; }
}

#endregion
