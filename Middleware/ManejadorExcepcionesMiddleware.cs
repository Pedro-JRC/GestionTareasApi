using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace GestorTareasApi.Middleware;

public class ManejadorExcepcionesMiddleware
{
    private readonly RequestDelegate _siguiente;
    private readonly ILogger<ManejadorExcepcionesMiddleware> _logger;

    public ManejadorExcepcionesMiddleware(RequestDelegate siguiente, ILogger<ManejadorExcepcionesMiddleware> logger)
    {
        _siguiente = siguiente;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext contexto)
    {
        try
        {
            await _siguiente(contexto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error capturado: {ex.Message}");

            contexto.Response.ContentType = "application/json";
            contexto.Response.StatusCode = StatusCodes.Status500InternalServerError;
            contexto.Response.Headers.Append("X-Error", "Error Interno del Servidor");

            var respuesta = new
            {
                estado = contexto.Response.StatusCode,
                mensaje = "Ha ocurrido un error en el servidor.",
                detalle = ex.Message
            };

            await contexto.Response.WriteAsync(JsonSerializer.Serialize(respuesta));
        }
    }
}
