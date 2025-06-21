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

public class _07_CrearTarea_SeEncolaCorrectamente
{
    [Fact]
    public async Task CrearAsync_TareaValida_RetornaConfirmacionEncolado()
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

        var dto = new CrearTareaDTO
        {
            Titulo = "Tarea de prueba",
            Descripcion = "Descripción válida",
            Estado = "Pendiente",
            FechaVencimiento = DateTime.UtcNow.AddDays(1),
            Categoria = "General",
            Prioridad = 2,
            AsignadoA = null
        };

        // ACT
        var (exitoso, mensaje, resultado) = await servicio.CrearAsync(dto);

        // ASSERT
        Assert.True(exitoso);
        Assert.Equal("Tarea agregada a la cola para ser procesada.", mensaje);
        Assert.Null(resultado);
    }
}
