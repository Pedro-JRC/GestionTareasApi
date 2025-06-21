using Xunit;
using GestionTareasApi.DTOs;
using GestionTareasApi.Models;
using GestionTareasApi.Servicios;
using System.Threading.Tasks;
using GestionTareasApi.Utilidades;
using System;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using xUnitTest;

public class _03_RefrescarToken_SiVaAExpirar
{
    [Fact]
    public void RefrescarSiVaAExpirar_TokenPorExpirar_RetornaNuevoToken()
    {
        // ARRANGE
        var entorno = new EntornoPruebas();
        var contexto = entorno.CrearContextoEnMemoria();
        var config = entorno.CrearConfiguracionFalsa();

        var usuario = new UsuariosModel
        {
            NombreUsuario = "UsuarioPruebas",
            Contraseña = SeguridadHelper.HashearContraseña("Pruebas1234"),
            Email = "usuario@pruebas.com",
            Nombre = "Usuario Pruebas",
            Rol = "Estudiante",
            Activo = true
        };
        contexto.Usuarios.Add(usuario);
        contexto.SaveChanges();

        var clave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
        var credenciales = new SigningCredentials(clave, SecurityAlgorithms.HmacSha256);

        // Se genera un token con 4 minutos de vigencia para forzar el refresco
        var tokenJwt = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: new[]
            {
                new Claim(ClaimTypes.Name, usuario.NombreUsuario),
                new Claim(ClaimTypes.Role, usuario.Rol)
            },
            expires: DateTime.UtcNow.AddMinutes(4),
            signingCredentials: credenciales
        );
        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenJwt);

        var servicio = new ServicioAutenticacion(contexto, config);

        // ACT
        var (nuevoToken, mensaje) = servicio.RefrescarSiVaAExpirar(tokenString);

        // ASSERT
        Assert.False(string.IsNullOrEmpty(nuevoToken));
        Assert.Equal("Token refrescado correctamente.", mensaje);
    }
}
