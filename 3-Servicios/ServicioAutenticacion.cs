using GestionTareasApi.Data;
using GestionTareasApi.DTOs;
using GestionTareasApi.Models;
using GestionTareasApi.Utilidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GestionTareasApi.Servicios;

public class ServicioAutenticacion
{
    private readonly AppDbContext _contexto;
    private readonly IConfiguration _configuracion;

    public ServicioAutenticacion(AppDbContext contexto, IConfiguration configuracion)
    {
        _contexto = contexto;
        _configuracion = configuracion;
    }

    #region INICIO DE SESIÓN

    public async Task<(bool Exitoso, string Mensaje, string Token)> IniciarSesionAsync(LoginDTO dto)
    {
        var usuario = await _contexto.Usuarios
            .FirstOrDefaultAsync(u =>
                u.NombreUsuario == dto.NombreUsuario &&
                u.Activo);

        if (usuario == null)
            return (false, "Credenciales inválidas o usuario inactivo.", string.Empty);

        var esValida = SeguridadHelper.VerificarContraseña(dto.Contraseña, usuario.Contraseña);
        if (!esValida)
            return (false, "Contraseña incorrecta.", string.Empty);

        var token = GenerarToken(usuario);
        return (true, "Inicio de sesión exitoso.", token);
    }

    #endregion

    #region GENERAR TOKEN JWT

    private string GenerarToken(UsuariosModel usuario)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Name, usuario.NombreUsuario),
            new Claim(ClaimTypes.Role, usuario.Rol)
        };

        var clave = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuracion["Jwt:Key"]));

        var credenciales = new SigningCredentials(clave, SecurityAlgorithms.HmacSha256);

        var minutos = int.Parse(_configuracion["Jwt:ExpiresInMinutes"]);

        var token = new JwtSecurityToken(
            issuer: _configuracion["Jwt:Issuer"],
            audience: _configuracion["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(minutos),
            signingCredentials: credenciales
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    #endregion

    #region REFRESCAR TOKEN

    public (string? Token, string? Mensaje) RefrescarSiVaAExpirar(string tokenJwt)
    {
        var handler = new JwtSecurityTokenHandler();
        var clave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuracion["Jwt:Key"]));

        var parametros = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _configuracion["Jwt:Issuer"],
            ValidAudience = _configuracion["Jwt:Audience"],
            IssuerSigningKey = clave,
            ValidateLifetime = false
        };

        try
        {
            var principal = handler.ValidateToken(tokenJwt, parametros, out SecurityToken tokenValidado);
            var jwtToken = tokenValidado as JwtSecurityToken;

            if (jwtToken == null)
                return (null, null);

            var tiempoRestante = jwtToken.ValidTo - DateTime.UtcNow;

            if (tiempoRestante.TotalMinutes > 5)
                return (null, null);

            var nombreUsuario = principal.Identity?.Name;
            if (string.IsNullOrEmpty(nombreUsuario))
                return (null, null);

            var usuario = _contexto.Usuarios.FirstOrDefault(u => u.NombreUsuario == nombreUsuario && u.Activo);
            if (usuario == null)
                return (null, null);

            var nuevoToken = GenerarToken(usuario);
            return (nuevoToken, "Token refrescado correctamente.");
        }
        catch
        {
            return (null, null);
        }
    }

    #endregion
}
