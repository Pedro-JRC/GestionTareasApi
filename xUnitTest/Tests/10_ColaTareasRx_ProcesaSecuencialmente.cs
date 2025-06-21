using Xunit;
using GestionTareasApi._3_Servicios;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using xUnitTest;

public class _10_ColaTareasRx_ProcesaSecuencialmente
{
    [Fact]
    public async Task Agregar_MultiplesTareas_ProcesaEnOrden()
    {
        // ARRANGE
        var procesadas = new List<int>();
        var logger = new NullLogger<ColaTareasRxService>();

        var cola = new ColaTareasRxService(logger, new DummyScopeFactory());

        // ACT
        cola.Agregar(async _ => { await Task.Delay(10); procesadas.Add(1); });
        cola.Agregar(async _ => { await Task.Delay(10); procesadas.Add(2); });
        cola.Agregar(async _ => { await Task.Delay(10); procesadas.Add(3); });

        await Task.Delay(500); // da tiempo al procesamiento

        // ASSERT
        Assert.Equal(new List<int> { 1, 2, 3 }, procesadas);
    }
}
