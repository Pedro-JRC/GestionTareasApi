using Microsoft.AspNetCore.Identity;

namespace GestionTareasApi.Utilidades;

public static class SeguridadHelper
{
    private static readonly PasswordHasher<object> _hasher = new();

    #region HASHEAR CONTRASEÑA

    /// <summary>
    /// Genera un hash seguro de la contraseña.
    /// </summary>
    public static string HashearContraseña(string contraseña)
    {
        return _hasher.HashPassword(null, contraseña);
    }

    #endregion

    #region VERIFICAR CONTRASEÑA

    /// <summary>
    /// Verifica si una contraseña ingresada coincide con el hash guardado.
    /// </summary>
    public static bool VerificarContraseña(string contraseñaIngresada, string hashAlmacenado)
    {
        var resultado = _hasher.VerifyHashedPassword(null, hashAlmacenado, contraseñaIngresada);
        return resultado == PasswordVerificationResult.Success;
    }

    #endregion
}
