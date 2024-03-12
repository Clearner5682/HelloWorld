using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1.DependencyInject
{
    public class ServiceLocator
    {
        private static ServiceLocator _serviceLocator=null;
        static ServiceLocator() 
        {
            _serviceLocator = new ServiceLocator();
        }

        public static ServiceLocator Instance
        {
            get
            {
                return _serviceLocator;
            }
        }

        private IServiceProvider _serviceProvider;
        public void SetServiceProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider= serviceProvider;
        }

        public T GetService<T>()
        {
            return this._serviceProvider.GetService<T>();
        }
    }
}
