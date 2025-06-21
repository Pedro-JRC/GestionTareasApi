using Xunit;
using GestionTareasApi.DTOs;
using GestionTareasApi.Models;
using GestionTareasApi.Servicios;
using GestionTareasApi.Utilidades;
using System.Threading.Tasks;
using xUnitTest;

public class _04_RegistrarUsuario_RechazaDuplicado
{
    [Fact]
    public async Task CrearAsync_DatosDuplicados_RetornaFallo()
    {
        // ARRANGE
        var entorno = new EntornoPruebas();
        var contexto = entorno.CrearContextoEnMemoria();

        contexto.Usuarios.Add(new UsuariosModel
        {
            NombreUsuario = "UsuarioPruebas",
            Email = "usuario@pruebas.com",
            Nombre = "Usuario Original",
            Contraseña = SeguridadHelper.HashearContraseña("Pruebas1234"),
            Rol = "Estudiante",
            Activo = true
        });
        await contexto.SaveChangesAsync();

        var servicio = new UsuariosService(contexto);

        var dto = new CrearUsuarioDTO
        {
            NombreUsuario = "UsuarioPruebas", // duplicado
            Email = "usuario@pruebas.com",    // duplicado
            Nombre = "Nuevo Usuario",
            Contraseña = "OtraClave123",
            Rol = "Estudiante"
        };

        // ACT
        var (exitoso, mensaje) = await servicio.CrearAsync(dto);

        // ASSERT
        Assert.False(exitoso);
        Assert.Equal("El nombre de usuario o email ya está en uso.", mensaje);
    }
}
