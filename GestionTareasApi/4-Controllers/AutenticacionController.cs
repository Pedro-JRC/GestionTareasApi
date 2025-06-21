using GestionTareasApi.DTOs;
using GestionTareasApi.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionTareasApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AutenticacionController : ControllerBase
{
    // INYECTA EL SERVICIO DE AUTENTICACIÓN
    private readonly ServicioAutenticacion _servicio;

    // CONSTRUCTOR DEL CONTROLADOR
    public AutenticacionController(ServicioAutenticacion servicio)
    {
        _servicio = servicio;
    }

    #region INICIAR SESIÓN

    /// <summary>
    /// REALIZA EL INICIO DE SESIÓN Y DEVUELVE UN TOKEN SI LAS CREDENCIALES SON VÁLIDAS
    /// </summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO dto)
    {
        // VALIDAR SI EL MODELO RECIBIDO ES VÁLIDO
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // INTENTA INICIAR SESIÓN CON EL SERVICIO DE AUTENTICACIÓN
        var (exitoso, mensaje, token) = await _servicio.IniciarSesionAsync(dto);

        // DEVUELVE UNAUTHORIZED SI FALLA EL LOGIN
        if (!exitoso)
            return Unauthorized(new { mensaje });

        // RETORNA OK CON EL TOKEN Y MENSAJE
        return Ok(new { mensaje, token });
    }

    #endregion

    #region REFRESCAR TOKEN

    /// <summary>
    /// REFRESCA EL TOKEN SI ESTÁ PRÓXIMO A EXPIRAR
    /// </summary>
    [Authorize]
    [HttpPost("refrescartoken")]
    public IActionResult Refrescar()
    {
        // OBTIENE EL TOKEN DEL HEADER
        var token = Request.Headers["Authorization"].ToString()?.Replace("Bearer ", "");

        // VERIFICA QUE EL TOKEN HAYA SIDO ENVIADO
        if (string.IsNullOrEmpty(token))
            return BadRequest(new { mensaje = "No se proporcionó el token." });

        // INTENTA REFRESCAR EL TOKEN SI VA A EXPIRAR
        var (nuevoToken, mensaje) = _servicio.RefrescarSiVaAExpirar(token);

        // SI NO HAY NUEVO TOKEN, EL ACTUAL AÚN ES VÁLIDO O ES INVÁLIDO
        if (string.IsNullOrEmpty(nuevoToken))
            return Unauthorized(new { mensaje = "El token aún no necesita ser refrescado o es inválido." });

        // RETORNA EL NUEVO TOKEN
        return Ok(new
        {
            mensaje,
            token = nuevoToken
        });
    }

    #endregion
}
