using GestionTareasApi.DTOs;

namespace GestionTareasApi.Delegados
{
    public static class DelegadosTarea
    {
        #region DELEGADOS PARA VALIDACIONES DE TAREAS

        // DELEGADO PARA VALIDAR UNA TAREA NUEVA
        public delegate (bool Exitoso, string? Mensaje) ValidadorTareaDelegate(CrearTareaDTO dto);

        // DELEGADO PARA VALIDAR UNA TAREA EXISTENTE ACTUALIZADA
        public delegate (bool Exitoso, string? Mensaje) ValidadorTareaActualizadaDelegate(ActualizarTareaDTO dto);

        #endregion
    }
}
