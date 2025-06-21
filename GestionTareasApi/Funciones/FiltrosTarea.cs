using GestionTareasApi.DTOs;
using GestionTareasApi.Utilidades;

namespace GestionTareasApi.Funciones
{
    public static class FiltrosTarea
    {
        #region FUNCIONES DE FILTRADO PARA TAREAS

        // FILTRA POR ESTADO (INSENSIBLE A MAYÚSCULAS/MINÚSCULAS)
        public static Func<TareaDTO, bool> PorEstado(string estado) =>
            t => t.Estado.Equals(estado, StringComparison.OrdinalIgnoreCase);

        // FILTRA POR FECHA DE VENCIMIENTO EXACTA
        public static Func<TareaDTO, bool> PorFecha(DateTime fecha) =>
            t => t.FechaVencimiento.Date == fecha.Date;

        // FILTRA POR ESTADO Y FECHA COMBINADOS
        public static Func<TareaDTO, bool> PorEstadoYFecha(string estado, DateTime fecha) =>
            t => t.Estado.Equals(estado, StringComparison.OrdinalIgnoreCase) &&
                 t.FechaVencimiento.Date == fecha.Date;

        // FILTRA POR PRIORIDAD GENERAL
        public static Func<TareaDTO, bool> PorPrioridad(int prioridad) =>
            t => t.Prioridad == prioridad;

        // FILTRA POR NOMBRE DE USUARIO ASIGNADO
        public static Func<TareaDTO, bool> PorAsignado(string usuario) =>
            t => !string.IsNullOrWhiteSpace(t.AsignadoA) &&
                 t.AsignadoA.Contains(usuario, StringComparison.OrdinalIgnoreCase);

        // FILTRA POR CATEGORÍA (BUSQUEDA PARCIAL INSENSIBLE A MAYÚSCULAS)
        public static Func<TareaDTO, bool> PorCategoria(string categoria) =>
            t => !string.IsNullOrWhiteSpace(t.Categoria) &&
                 t.Categoria.Contains(categoria, StringComparison.OrdinalIgnoreCase);

        // FILTRA POR ETIQUETA EN DATOSADICIONALES (LISTA DE TEXTO)
        public static Func<TareaDTO, bool> PorEtiqueta(string etiqueta) =>
            t =>
            {
                var lista = DatosAdicionalesHelper.ComoListaDeTexto(t.DatosAdicionales);
                return lista != null && lista.Any(e => e.Equals(etiqueta, StringComparison.OrdinalIgnoreCase));
            };

        // FILTRA POR PRIORIDAD EN DATOSADICIONALES (NÚMERO ENTERO)
        public static Func<TareaDTO, bool> PorPrioridadEnDatos(int valor) =>
            t =>
            {
                var numero = DatosAdicionalesHelper.ComoEntero(t.DatosAdicionales);
                return numero.HasValue && numero.Value == valor;
            };

        #endregion
    }
}
