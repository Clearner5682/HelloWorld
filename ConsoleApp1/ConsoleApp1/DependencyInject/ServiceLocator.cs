using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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

        public void Intercept(Type serviceType,Type implementType)
        {
            if(OnRegistered!=null)
            {
                OnRegistered(serviceType,implementType);
            }
        }

        public T GetService<T>()
        {
            return this._serviceProvider.GetService<T>();
        }

        public event Action<Type, Type> OnRegistered;
    }
}
