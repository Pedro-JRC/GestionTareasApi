using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace GestorTareasApi.Middleware
{
    public class ManejadorExcepcionesMiddleware
    {
        #region CONSTRUCTOR Y DEPENDENCIAS

        // DELEGADO DEL SIGUIENTE COMPONENTE EN LA TUBERÍA DE MIDDLEWARE
        private readonly RequestDelegate _siguiente;

        // LOGGER PARA REGISTRAR LOS ERRORES
        private readonly ILogger<ManejadorExcepcionesMiddleware> _logger;

        // CONSTRUCTOR DEL MIDDLEWARE
        public ManejadorExcepcionesMiddleware(RequestDelegate siguiente, ILogger<ManejadorExcepcionesMiddleware> logger)
        {
            _siguiente = siguiente;
            _logger = logger;
        }

        #endregion

        #region INVOCACIÓN DEL MIDDLEWARE

        /// <summary>
        /// MÉTODO PRINCIPAL QUE INTERCEPTA LAS SOLICITUDES Y MANEJA LAS EXCEPCIONES
        /// </summary>
        public async Task InvokeAsync(HttpContext contexto)
        {
            try
            {
                // EJECUTA EL SIGUIENTE MIDDLEWARE EN LA CADENA
                await _siguiente(contexto);
            }
            catch (Exception ex)
            {
                // REGISTRA EL ERROR EN LOS LOGS
                _logger.LogError(ex, $"Error capturado: {ex.Message}");

                // CONFIGURA LA RESPUESTA DE ERROR EN FORMATO JSON
                contexto.Response.ContentType = "application/json";
                contexto.Response.StatusCode = StatusCodes.Status500InternalServerError;
                contexto.Response.Headers.Append("X-Error", "Error Interno del Servidor");

                var respuesta = new
                {
                    estado = contexto.Response.StatusCode,
                    mensaje = "Ha ocurrido un error en el servidor.",
                    detalle = ex.Message
                };

                // DEVUELVE LA RESPUESTA SERIALIZADA AL CLIENTE
                await contexto.Response.WriteAsync(JsonSerializer.Serialize(respuesta));
            }
        }

        #endregion
    }
}
