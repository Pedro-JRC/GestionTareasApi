using Xunit;
using GestionTareasApi.DTOs;
using GestionTareasApi.Servicios;
using GestionTareasApi._3_Servicios;
using Microsoft.AspNetCore.SignalR;
using GestionTareasApi.SignalR;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Threading.Tasks;
using xUnitTest;

public class _08_ActualizarTarea_NoExiste
{
    [Fact]
    public async Task ActualizarAsync_TareaNoExiste_NoSeActualizaNada()
    {
        // ARRANGE
        var entorno = new EntornoPruebas();
        var contexto = entorno.CrearContextoEnMemoria();

        var colaReal = new ColaTareasRxService(
            new NullLogger<ColaTareasRxService>(),
            new DummyScopeFactory()
        );

        var memorizador = new MemorizadorTareasService(new NullLogger<MemorizadorTareasService>());
        var mockHub = new Moq.Mock<IHubContext<AppHub>>();

        var servicio = new TareasService(contexto, colaReal, memorizador, mockHub.Object);

        var dto = new ActualizarTareaDTO
        {
            Id = 9999, // tarea inexistente
            Titulo = "Título actualizado",
            Descripcion = "Descripción actualizada",
            Estado = "Completado",
            FechaVencimiento = DateTime.UtcNow.AddDays(2),
            Prioridad = 1,
            Categoria = "Actualizada",
            AsignadoA = null,
            EstaActiva = true,
            FechaCompletado = null
        };

        // ACT
        var (exitoso, mensaje) = await servicio.ActualizarAsync(dto);

        // ASSERT
        Assert.True(exitoso);
        Assert.Equal("Tarea encolada para ser actualizada.", mensaje);
    }
}
