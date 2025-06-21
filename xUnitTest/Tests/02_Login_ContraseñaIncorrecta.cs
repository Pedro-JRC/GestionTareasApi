using Xunit;
using GestionTareasApi.DTOs;
using GestionTareasApi.Models;
using GestionTareasApi.Servicios;
using System.Threading.Tasks;
using GestionTareasApi.Utilidades;
using xUnitTest;

public class _02_Login_ContraseñaIncorrecta
{
    [Fact]
    public async Task IniciarSesionAsync_ContrasenaIncorrecta_RetornaFallo()
    {
        // ARRANGE
        var entorno = new EntornoPruebas();
        var contexto = entorno.CrearContextoEnMemoria();
        var config = entorno.CrearConfiguracionFalsa();

        var hash = SeguridadHelper.HashearContraseña("Pruebas1234"); 

        contexto.Usuarios.Add(new UsuariosModel
        {
            NombreUsuario = "UsuarioPruebas",
            Contraseña = hash,
            Email = "usuario@pruebas.com",
            Nombre = "Usuario Pruebas",
            Rol = "Estudiante",
            Activo = true
        });
        await contexto.SaveChangesAsync();

        var servicio = new ServicioAutenticacion(contexto, config);
        var dto = new LoginDTO
        {
            NombreUsuario = "UsuarioPruebas",
            Contraseña = "Pruebas123456789"
        };

        // ACT
        var (exitoso, mensaje, token) = await servicio.IniciarSesionAsync(dto);

        // ASSERT
        Assert.False(exitoso);
        Assert.True(string.IsNullOrEmpty(token));
        Assert.Equal("Contraseña incorrecta.", mensaje);
    }
}
