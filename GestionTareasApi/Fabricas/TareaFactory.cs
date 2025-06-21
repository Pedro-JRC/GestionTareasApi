using GestionTareasApi.Models;

namespace GestionTareasApi.Fabricas
{
    public static class TareaFactory
    {
        // CREA UNA TAREA DE ALTA PRIORIDAD
        public static TareaGeneral CrearTareaAltaPrioridad(string titulo, string descripcion, string asignadoA = "")
        {
            return new TareaGeneral
            {
                Titulo = titulo,
                Descripcion = descripcion,
                FechaVencimiento = DateTime.UtcNow.AddDays(1),
                Estado = "Pendiente",
                DatosAdicionales = "[\"alta\", \"urgente\"]",
                Prioridad = 3,
                Categoria = "Urgente",
                AsignadoA = asignadoA
            };
        }

        // CREA UNA TAREA DOCUMENTAL
        public static TareaGeneral CrearTareaDocumentacion(string titulo, string descripcion, string asignadoA = "")
        {
            return new TareaGeneral
            {
                Titulo = titulo,
                Descripcion = descripcion,
                FechaVencimiento = DateTime.UtcNow.AddDays(5),
                Estado = "En progreso",
                DatosAdicionales = "[\"documentacion\", \"especificaciones\"]",
                Prioridad = 2,
                Categoria = "Documentación",
                AsignadoA = asignadoA
            };
        }

        // CREA UNA TAREA URGENTE 
        public static TareaGeneral CrearTareaUrgente(string titulo, string descripcion, string asignadoA = "")
        {
            return new TareaGeneral
            {
                Titulo = titulo,
                Descripcion = descripcion,
                FechaVencimiento = DateTime.UtcNow,
                Estado = "Pendiente",
                DatosAdicionales = "[\"urgente\", \"critico\"]",
                Prioridad = 4,
                Categoria = "Crítica",
                AsignadoA = asignadoA
            };
        }
    }
}
