using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Reactive.Subjects;

namespace GestionTareasApi._3_Servicios
{
    public class ColaTareasRxService
    {
        #region CAMPOS PRIVADOS

        // FLUJO REACTIVO DE TAREAS ENTRANTES
        private readonly Subject<Func<Task>> _flujoTareas = new();

        // COLA CONCURRENTE DE TAREAS PENDIENTES (FIFO)
        private readonly ConcurrentQueue<Func<Task>> _cola = new();

        // LOGGER PARA REGISTRO DE PROCESOS
        private readonly ILogger<ColaTareasRxService> _logger;

        // INDICA SI SE ESTÁ PROCESANDO UNA TAREA ACTUALMENTE
        private bool _procesando = false;

        #endregion

        #region CONSTRUCTOR

        /// <summary>
        /// INICIALIZA LA COLA Y SUSCRIBE AL FLUJO DE TAREAS PARA PROCESARLAS DE FORMA SECUENCIAL
        /// </summary>
        public ColaTareasRxService(ILogger<ColaTareasRxService> logger)
        {
            _logger = logger;

            // SUSCRIBE EL FLUJO DE TAREAS AL PROCESADOR
            _flujoTareas.Subscribe(async tarea =>
            {
                if (_procesando) return;
                await ProcesarSiguienteAsync();
            });
        }

        #endregion

        #region MÉTODO PARA AGREGAR TAREA A LA COLA

        /// <summary>
        /// AGREGA UNA NUEVA TAREA A LA COLA Y DISPARA EL FLUJO REACTIVO PARA INICIAR SU PROCESAMIENTO
        /// </summary>
        public void Agregar(Func<Task> tarea)
        {
            _cola.Enqueue(tarea);
            _flujoTareas.OnNext(tarea);
            _logger.LogInformation("TAREA AGREGADA A LA COLA. TOTAL PENDIENTES: {0}", _cola.Count);
        }

        #endregion

        #region MÉTODO PARA PROCESAR LAS TAREAS EN ORDEN

        /// <summary>
        /// PROCESA UNA A UNA LAS TAREAS DE LA COLA EN ORDEN (FIFO), SIN SOLAPARSE
        /// </summary>
        private async Task ProcesarSiguienteAsync()
        {
            if (_cola.IsEmpty) return;

            _procesando = true;

            while (_cola.TryDequeue(out var tarea))
            {
                try
                {
                    _logger.LogInformation("PROCESANDO TAREA...");
                    await tarea.Invoke();
                    _logger.LogInformation("TAREA COMPLETADA.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "ERROR AL PROCESAR TAREA.");
                }
            }

            _procesando = false;
        }

        #endregion
    }

}
