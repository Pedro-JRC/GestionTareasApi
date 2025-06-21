using Xunit;
using GestionTareasApi.DTOs;
using GestionTareasApi.Servicios;

public class _09_ValidarDatosBasicos_DescripcionMuyCorta
{
    [Fact]
    public void ValidarDatosBasicos_DescripcionCorta_RetornaFalso()
    {
        // ARRANGE
        var dto = new CrearTareaDTO
        {
            Titulo = "Corto",
            Descripcion = "1234", // solo 4 caracteres
            Estado = "Pendiente",
            FechaVencimiento = DateTime.UtcNow.AddDays(1),
            Categoria = "General",
            Prioridad = 2
        };

        // ACT
        var (valido, mensaje) = ValidacionesTareaService.ValidarDatosBasicos(dto);

        // ASSERT
        Assert.False(valido);
        Assert.Equal("La descripción debe tener al menos 5 caracteres.", mensaje);
    }
}
