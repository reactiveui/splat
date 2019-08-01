using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Splat.Microsoft.Extensions.DependencyInjection.Tests
{
    internal class ContainerWrapper
    {
        private IServiceProvider _serviceProvider;

        public ContainerWrapper()
        {
            ServiceCollection.UseMicrosoftDependencyResolver();
        }

        public IServiceCollection ServiceCollection { get; } = new ServiceCollection();

        public IServiceProvider ServiceProvider
        {
            get
            {
                if (_serviceProvider == null)
                {
                    _serviceProvider = ServiceCollection.BuildServiceProvider();
                }

                return _serviceProvider;
            }
        }

        public void BuildAndUse() => ServiceProvider.UseMicrosoftDependencyResolver();
    }
}
