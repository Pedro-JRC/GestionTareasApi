using GestionTareasApi.DTOs;

namespace GestionTareasApi.Funciones
{
    public static class FuncionesTarea
    {
        #region FUNCIONES AUXILIARES PARA TAREAS

        // CALCULA LOS DÍAS RESTANTES HASTA LA FECHA DE VENCIMIENTO
        // SI LA FECHA YA PASÓ, DEVUELVE 0
        public static readonly Func<TareaDTO, int> CalcularDiasRestantes = tarea =>
        {
            var dias = (tarea.FechaVencimiento.Date - DateTime.UtcNow.Date).Days;
            return dias < 0 ? 0 : dias;
        };

        #endregion
    }
}
