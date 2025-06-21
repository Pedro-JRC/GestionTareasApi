using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xUnitTest
{
    public class DummyScopeFactory : IServiceScopeFactory
    {
        public IServiceScope CreateScope() => new DummyScope();

        private class DummyScope : IServiceScope
        {
            public IServiceProvider ServiceProvider => new DummyProvider();
            public void Dispose() { }

            private class DummyProvider : IServiceProvider
            {
                public object? GetService(Type serviceType) => null!;
            }
        }
    }
}
