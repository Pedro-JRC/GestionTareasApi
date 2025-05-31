using Microsoft.Extensions.Logging;

namespace GestionTareasApi._3_Servicios
{
    public class MemorizadorTareasService
    {
        private readonly Dictionary<string, double> _cachePorcentaje = new();
        private readonly ILogger<MemorizadorTareasService> _logger;

        public MemorizadorTareasService(ILogger<MemorizadorTareasService> logger)
        {
            _logger = logger;
        }

        public bool TryObtenerPorcentaje(string clave, out double resultado)
        {
            if (_cachePorcentaje.TryGetValue(clave, out resultado))
            {
                _logger.LogInformation("[MEMORIZADOR] Cache HIT para clave: {Clave}", clave);
                return true;
            }

            _logger.LogInformation("[MEMORIZADOR] Cache MISS para clave: {Clave}", clave);
            return false;
        }

        public void GuardarPorcentaje(string clave, double resultado)
        {
            _cachePorcentaje[clave] = resultado;
            _logger.LogInformation("[MEMORIZADOR] Porcentaje guardado en caché con clave: {Clave}", clave);
        }
    }
}
