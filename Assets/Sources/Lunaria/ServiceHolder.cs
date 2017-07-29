using System;
using System.Collections.Generic;

namespace Lunaria
{
    public class ServiceHolder
    {
        private readonly Dictionary<Type, object> _holder = new Dictionary<Type, object>();
        public static readonly ServiceHolder Instance = new ServiceHolder();

        private ServiceHolder() {}

        public void Hold<T>(T service)
        {
            _holder[typeof(T)] = service;
        }

        public T Get<T>()
        {
            return (T)_holder[typeof(T)];
        }

        public void Clear()
        {
            _holder.Clear();
        }
    }
}
