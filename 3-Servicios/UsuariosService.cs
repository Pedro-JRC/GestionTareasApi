using GestionTareasApi.Data;
using GestionTareasApi.DTOs;
using GestionTareasApi.Models;
using GestionTareasApi.Utilidades;
using Microsoft.EntityFrameworkCore;

namespace GestionTareasApi.Servicios;

public class UsuariosService
{
    private readonly AppDbContext _contexto;

    public UsuariosService(AppDbContext contexto)
    {
        _contexto = contexto;
    }

    #region OBTENER TODOS LOS USUARIOS

    public async Task<List<UsuariosDTO>> ObtenerTodosAsync()
    {
        var usuarios = await _contexto.Usuarios.ToListAsync();
        return usuarios.Select(MapearADTO).ToList();
    }

    #endregion

    #region OBTENER USUARIO POR ID

    public async Task<UsuariosDTO?> ObtenerPorIdAsync(int id)
    {
        var usuario = await _contexto.Usuarios.FindAsync(id);
        return usuario == null ? null : MapearADTO(usuario);
    }

    #endregion

    #region CREAR NUEVO USUARIO

    public async Task<(bool Exitoso, string Mensaje)> CrearAsync(CrearUsuarioDTO dto)
    {
        var existe = await _contexto.Usuarios.AnyAsync(u =>
            u.NombreUsuario.ToLower() == dto.NombreUsuario.ToLower() ||
            u.Email.ToLower() == dto.Email.ToLower());

        if (existe)
            return (false, "El nombre de usuario o email ya está en uso.");

        var nuevo = new UsuariosModel
        {
            NombreUsuario = dto.NombreUsuario,
            Nombre = dto.Nombre,
            Email = dto.Email,
            Contraseña = SeguridadHelper.HashearContraseña(dto.Contraseña),
            Rol = dto.Rol,
            Activo = true
        };

        _contexto.Usuarios.Add(nuevo);
        await _contexto.SaveChangesAsync();

        return (true, "Usuario registrado correctamente.");
    }

    #endregion

    #region ACTUALIZAR USUARIO

    public async Task<(bool Exitoso, string Mensaje)> ActualizarAsync(ActualizarUsuarioDTO dto)
    {
        if (dto.Contraseña != dto.ConfirmarContraseña)
            return (false, "Las contraseñas no coinciden.");

        var usuario = await _contexto.Usuarios.FindAsync(dto.Id);
        if (usuario == null)
            return (false, "Usuario no encontrado.");

        var duplicado = await _contexto.Usuarios.AnyAsync(u =>
            u.Id != dto.Id &&
            (u.NombreUsuario.ToLower() == dto.NombreUsuario.ToLower() ||
             u.Email.ToLower() == dto.Email.ToLower()));

        if (duplicado)
            return (false, "Otro usuario ya está utilizando ese nombre de usuario o email.");

        usuario.NombreUsuario = dto.NombreUsuario;
        usuario.Nombre = dto.Nombre;
        usuario.Email = dto.Email;
        usuario.Contraseña = SeguridadHelper.HashearContraseña(dto.Contraseña);
        usuario.Rol = dto.Rol;
        usuario.Activo = dto.Activo;

        await _contexto.SaveChangesAsync();
        return (true, "Usuario actualizado correctamente.");
    }

    #endregion


    #region ELIMINAR USUARIO

    public async Task<(bool Exitoso, string Mensaje)> EliminarAsync(int id)
    {
        var usuario = await _contexto.Usuarios.FindAsync(id);
        if (usuario == null)
            return (false, "Usuario no encontrado.");

        _contexto.Usuarios.Remove(usuario);
        await _contexto.SaveChangesAsync();

        return (true, "Usuario eliminado correctamente.");
    }

    #endregion

    #region MAPEO A DTO

    private UsuariosDTO MapearADTO(UsuariosModel usuario)
    {
        return new UsuariosDTO
        {
            Id = usuario.Id,
            NombreUsuario = usuario.NombreUsuario,
            Nombre = usuario.Nombre,
            Email = usuario.Email,
            Rol = usuario.Rol,
            Activo = usuario.Activo
        };
    }

    #endregion
}
