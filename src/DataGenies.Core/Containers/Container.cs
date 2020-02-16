using System;
using System.Collections.Generic;

namespace DataGenies.Core.Containers
{
    public class Container : IContainer
    {
        private readonly Dictionary<Type, Dictionary<string, object>> _containerBag =
            new Dictionary<Type, Dictionary<string, object>>();

        public void Register<T>(object instance) where T : class
        {
            if (_containerBag.ContainsKey(typeof(T)))
            {
                throw new ArgumentException(
                    "Can't add type because it exists already. Please define name for another instance",
                    paramName: nameof(instance));
            }
            
            this._containerBag.Add(typeof(T), new Dictionary<string, object>()
            {
                { string.Empty, instance}
            });
        }

        public void Register<T>(object instance, string name) where T : class
        {
            if (_containerBag.ContainsKey(typeof(T)) && _containerBag[typeof(T)].ContainsKey(name))
            {
                throw new ArgumentException("Can't add type because it exists already with this name",
                    paramName: nameof(name));
            }

            if (!_containerBag.ContainsKey(typeof(T)))
            {
                this._containerBag.Add(typeof(T), new Dictionary<string, object>());
            }
            
            this._containerBag[typeof(T)][name] = instance;
        }

        public T Resolve<T>()
        {
            return (T)this._containerBag[typeof(T)][string.Empty];
        }

        public T Resolve<T>(string name)
        {
            return (T)this._containerBag[typeof(T)][name];
        }
    }
}