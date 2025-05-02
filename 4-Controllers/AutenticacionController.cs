using GestionTareasApi.DTOs;
using GestionTareasApi.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionTareasApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AutenticacionController : ControllerBase
{
    private readonly ServicioAutenticacion _servicio;

    public AutenticacionController(ServicioAutenticacion servicio)
    {
        _servicio = servicio;
    }

    #region INICIAR SESIÓN

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var (exitoso, mensaje, token) = await _servicio.IniciarSesionAsync(dto);

        if (!exitoso)
            return Unauthorized(new { mensaje });

        return Ok(new { mensaje, token });
    }

    #endregion

    #region REFRESCAR TOKEN
    [Authorize]
    [HttpPost("refrescartoken")]
    public IActionResult Refrescar()
    {
        var token = Request.Headers["Authorization"].ToString()?.Replace("Bearer ", "");

        if (string.IsNullOrEmpty(token))
            return BadRequest(new { mensaje = "No se proporcionó el token." });

        var (nuevoToken, mensaje) = _servicio.RefrescarSiVaAExpirar(token);

        if (string.IsNullOrEmpty(nuevoToken))
            return Unauthorized(new { mensaje = "El token aún no necesita ser refrescado o es inválido." });

        return Ok(new
        {
            mensaje,
            token = nuevoToken
        });
    }



    #endregion
}
