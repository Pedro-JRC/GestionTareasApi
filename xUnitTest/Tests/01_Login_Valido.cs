using Xunit;
using GestionTareasApi.DTOs;
using GestionTareasApi.Models;
using GestionTareasApi.Servicios;
using System.Threading.Tasks;
using GestionTareasApi.Utilidades;
using xUnitTest;

public class _01_Login_Valido
{
    [Fact]
    public async Task IniciarSesionAsync_CredencialesValidas_RetornaToken()
    {
        // ARRANGE
        var entorno = new EntornoPruebas();
        var contexto = entorno.CrearContextoEnMemoria();
        var config = entorno.CrearConfiguracionFalsa();

        var contraseña = "Pruebas1234";
        var hash = SeguridadHelper.HashearContraseña(contraseña);

        contexto.Usuarios.Add(new UsuariosModel
        {
            NombreUsuario = "UsuarioPruebas",
            Contraseña = hash,
            Email = "UsuarioPruebas@correo.com",
            Nombre = "Usuario Pruebas",
            Rol = "Estudiante",
            Activo = true
        });
        await contexto.SaveChangesAsync();


        var servicio = new ServicioAutenticacion(contexto, config);
        var dto = new LoginDTO
        {
            NombreUsuario = "UsuarioPruebas",
            Contraseña = contraseña
        };

        // ACT
        var (exitoso, mensaje, token) = await servicio.IniciarSesionAsync(dto);

        // ASSERT
        Assert.True(exitoso);
        Assert.False(string.IsNullOrEmpty(token));
    }
}
