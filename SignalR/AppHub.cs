using Microsoft.AspNetCore.SignalR;

namespace GestionTareasApi.SignalR
{
    public class AppHub : Hub
    {
        // ESTE MÉTODO PUEDE USARSE PARA ENVIAR MENSAJES A TODOS LOS CLIENTES
        public async Task NotificarNuevaTarea(string mensaje)
        {
            await Clients.All.SendAsync("RecibirNotificacion", mensaje);
        }
    }
}
