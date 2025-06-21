using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using GestionTareasApi.Data;

namespace xUnitTest
{
    public class EntornoPruebas
    {
        public AppDbContext CrearContextoEnMemoria()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: $"DbTest_{Guid.NewGuid()}")
                .Options;

            return new AppDbContext(options);
        }

        public IConfiguration CrearConfiguracionFalsa()
        {
            var configuracion = new Dictionary<string, string>
            {
                { "Jwt:Key", "e6441cef81b9ec466d5affbe007c6e861c12610b6a3852bebc407b08cccb6b90" },
                { "Jwt:Issuer", "test_issuer" },
                { "Jwt:Audience", "test_audience" },
                { "Jwt:ExpiresInMinutes", "30" }
            };

            return new ConfigurationBuilder()
                .AddInMemoryCollection(configuracion)
                .Build();
        }

        public ILogger<T> CrearLoggerFalso<T>()
        {
            return new LoggerFactory().CreateLogger<T>();
        }
    }
}
