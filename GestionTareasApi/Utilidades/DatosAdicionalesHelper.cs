using System.Text.Json;

namespace GestionTareasApi.Utilidades;

public static class DatosAdicionalesHelper
{
    #region UTILIDAD: DESERIALIZAR DATOSADICIONALES COMO LISTA

    /// <summary>
    /// Intenta deserializar el campo DatosAdicionales como una lista de cadenas.
    /// </summary>
    /// <param name="datosAdicionales">Texto JSON almacenado en el modelo.</param>
    /// <returns>Lista de cadenas o null si no es JSON válido.</returns>
    public static List<string>? ComoListaDeTexto(string? datosAdicionales)
    {
        if (string.IsNullOrWhiteSpace(datosAdicionales))
            return null;

        try
        {
            return JsonSerializer.Deserialize<List<string>>(datosAdicionales);
        }
        catch
        {
            return null;
        }
    }

    #endregion

    #region UTILIDAD: CONVERTIR DATOSADICIONALES A ENTERO

    /// <summary>
    /// Intenta convertir DatosAdicionales a entero (por ejemplo, para prioridad).
    /// </summary>
    /// <param name="datosAdicionales">Texto almacenado como string.</param>
    /// <returns>El número convertido o null si no es válido.</returns>
    public static int? ComoEntero(string? datosAdicionales)
    {
        if (string.IsNullOrWhiteSpace(datosAdicionales))
            return null;

        return int.TryParse(datosAdicionales, out int numero) ? numero : null;
    }

    #endregion
}
