using GestionTareasApi.DTOs;

namespace GestionTareasApi.Servicios
{
    public static class ValidacionesTareaService
    {
        #region VALIDACIÓN DE DATOS AL CREAR TAREA

        /// <summary>
        /// VALIDA LOS CAMPOS BÁSICOS AL CREAR UNA TAREA NUEVA
        /// </summary>
        public static (bool, string?) ValidarDatosBasicos(CrearTareaDTO dto)
        {
            // VALIDAR QUE LA DESCRIPCIÓN NO ESTÉ VACÍA
            if (string.IsNullOrWhiteSpace(dto.Descripcion))
                return (false, "La descripción es obligatoria.");

            // VALIDAR QUE LA DESCRIPCIÓN TENGA MÍNIMO 5 CARACTERES
            if (dto.Descripcion.Length < 5)
                return (false, "La descripción debe tener al menos 5 caracteres.");

            // VALIDAR QUE LA FECHA DE VENCIMIENTO NO SEA ANTERIOR A HOY
            if (dto.FechaVencimiento < DateTime.UtcNow.Date)
                return (false, "La fecha de vencimiento no puede ser anterior a hoy.");

            // VALIDAR QUE EL ESTADO NO ESTÉ VACÍO
            if (string.IsNullOrWhiteSpace(dto.Estado))
                return (false, "El estado de la tarea es obligatorio.");

            // TODO ES VÁLIDO
            return (true, null);
        }

        #endregion

        #region VALIDACIÓN DE DATOS AL ACTUALIZAR TAREA

        /// <summary>
        /// VALIDA LOS CAMPOS AL ACTUALIZAR UNA TAREA EXISTENTE
        /// </summary>
        public static (bool, string?) ValidarDatosActualizados(ActualizarTareaDTO dto)
        {
            // VALIDAR QUE LA DESCRIPCIÓN NO ESTÉ VACÍA
            if (string.IsNullOrWhiteSpace(dto.Descripcion))
                return (false, "La descripción es obligatoria.");

            // VALIDAR QUE LA DESCRIPCIÓN TENGA MÍNIMO 5 CARACTERES
            if (dto.Descripcion.Length < 5)
                return (false, "La descripción debe tener al menos 5 caracteres.");

            // VALIDAR QUE LA FECHA DE VENCIMIENTO NO SEA ANTERIOR A HOY
            if (dto.FechaVencimiento < DateTime.UtcNow.Date)
                return (false, "La fecha de vencimiento no puede ser anterior a hoy.");

            // VALIDAR QUE EL ESTADO NO ESTÉ VACÍO
            if (string.IsNullOrWhiteSpace(dto.Estado))
                return (false, "El estado de la tarea es obligatorio.");

            // TODO ES VÁLIDO
            return (true, null);
        }

        #endregion
    }
}
