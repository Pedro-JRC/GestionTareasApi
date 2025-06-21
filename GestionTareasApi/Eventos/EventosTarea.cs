using GestionTareasApi.DTOs;
using Microsoft.Extensions.Logging;

namespace GestionTareasApi.Eventos
{
    public static class EventosTarea
    {
        // LOGGER ESTÁTICO PARA REGISTRAR EVENTOS
        private static ILogger? _logger;

        #region CONFIGURAR LOGGER

        /// <summary>
        /// CONFIGURA EL LOGGER QUE SE UTILIZARÁ PARA REGISTRAR EVENTOS
        /// </summary>
        public static void ConfigurarLogger(ILogger logger)
        {
            _logger = logger;
        }

        #endregion

        #region GENERAR MENSAJE DE EVENTO

        /// <summary>
        /// GENERA UN MENSAJE DESCRIPTIVO PARA UNA TAREA
        /// </summary>
        public static string GenerarMensaje(TareaDTO tarea)
        {
            return $"TAREA → ID: {tarea.Id} | Estado: {tarea.Estado} | AsignadoA: {tarea.AsignadoA ?? "Sin asignar"} | Vence: {tarea.FechaVencimiento:yyyy-MM-dd}";
        }

        #endregion

        #region REGISTRAR EVENTO

        /// <summary>
        /// REGISTRA EL EVENTO EN EL LOGGER SI ESTÁ CONFIGURADO
        /// </summary>
        public static readonly Action<TareaDTO> RegistrarEvento = tarea =>
        {
            // SI NO HAY LOGGER CONFIGURADO, SALE SIN HACER NADA
            if (_logger is null) return;

            // GENERA EL MENSAJE Y LO REGISTRA
            var mensaje = GenerarMensaje(tarea);
            _logger.LogInformation(mensaje);
        };

        #endregion
    }
}
