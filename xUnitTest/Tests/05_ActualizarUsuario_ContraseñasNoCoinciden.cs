using Xunit;
using GestionTareasApi.DTOs;
using GestionTareasApi.Models;
using GestionTareasApi.Servicios;
using GestionTareasApi.Utilidades;
using System.Threading.Tasks;
using xUnitTest;

public class _05_ActualizarUsuario_ContraseñasNoCoinciden
{
    [Fact]
    public async Task ActualizarAsync_ContraseñasNoCoinciden_RetornaFallo()
    {
        // ARRANGE
        var entorno = new EntornoPruebas();
        var contexto = entorno.CrearContextoEnMemoria();

        var usuario = new UsuariosModel
        {
            NombreUsuario = "UsuarioPruebas",
            Email = "usuario@pruebas.com",
            Nombre = "Usuario Original",
            Contraseña = SeguridadHelper.HashearContraseña("Pruebas1234"),
            Rol = "Estudiante",
            Activo = true
        };
        contexto.Usuarios.Add(usuario);
        await contexto.SaveChangesAsync();

        var servicio = new UsuariosService(contexto);

        var dto = new ActualizarUsuarioDTO
        {
            Id = usuario.Id,
            NombreUsuario = "UsuarioPruebas",
            Email = "usuario@pruebas.com",
            Nombre = "Usuario Actualizado",
            Contraseña = "NuevaClave123",
            ConfirmarContraseña = "ClaveDiferente456", // no coinciden
            Rol = "Estudiante",
            Activo = true
        };

        // ACT
        var (exitoso, mensaje) = await servicio.ActualizarAsync(dto);

        // ASSERT
        Assert.False(exitoso);
        Assert.Equal("Las contraseñas no coinciden.", mensaje);
    }
}
