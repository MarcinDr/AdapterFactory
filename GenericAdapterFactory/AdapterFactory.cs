using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using GenericAdapterFactory.Interfaces;

namespace GenericAdapterFactory
{
	public class AdapterFactory : IAdapterFactory
	{
		private readonly IDictionary<Type, object> _adapters = new ConcurrentDictionary<Type, object>();
		
		public void RegisterAdapter<TAdapter>(TAdapter adapter)
		{
			if (!typeof(TAdapter).IsAssignableFrom(typeof(IGenericAdapter<>)))
			{
				throw new InvalidOperationException($"Adapter {typeof(TAdapter)} is not assignable from IGenericAdapter");
			}
			
			if (_adapters.ContainsKey(typeof(TAdapter)))
			{
				throw new InvalidOperationException($"Adapter {typeof(TAdapter)} is already defined");
			}
			_adapters.Add(typeof(TAdapter), adapter);
		}

		public TAdapter ResolveAdapter<TAdapter>()
		{
			object value;
			if (!_adapters.TryGetValue(typeof(TAdapter), out value))
			{
				throw new InvalidOperationException($"Adapter {typeof(TAdapter)} is not defined");
			}
			return (TAdapter) value;
		}

		public bool RemoveAdapter<TAdapter>()
		{
			return _adapters.Remove(typeof(TAdapter));
		}
	}
}
