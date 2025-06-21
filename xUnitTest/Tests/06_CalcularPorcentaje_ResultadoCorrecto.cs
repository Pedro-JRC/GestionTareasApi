using Xunit;
using GestionTareasApi.DTOs;
using GestionTareasApi.Servicios;
using GestionTareasApi._3_Servicios;
using Microsoft.Extensions.Logging.Abstractions;
using System.Collections.Generic;

public class _06_CalcularPorcentaje_ResultadoCorrecto
{
    [Fact]
    public void CalcularPorcentajeCompletadas_TareasMixtas_RetornaPorcentajeEsperado()
    {
        // ARRANGE
        var memorizador = new MemorizadorTareasService(new NullLogger<MemorizadorTareasService>());

        var tareas = new List<TareaDTO>
        {
            new TareaDTO { Id = 1, Estado = "Completado" },
            new TareaDTO { Id = 2, Estado = "Pendiente" },
            new TareaDTO { Id = 3, Estado = "Completado" },
            new TareaDTO { Id = 4, Estado = "En Proceso" }
        };

        var tareasService = new TareasService(
            context: null, // no se necesita para esta prueba
            cola: null,
            memorizador: memorizador,
            hub: null
        );

        // ACT
        var porcentaje = tareasService.CalcularPorcentajeCompletadas(tareas);

        // ASSERT
        Assert.Equal("50%", porcentaje);
    }
}
