using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using GenericAdapterFactory.Interfaces;

namespace GenericAdapterFactory
{
	public class AdapterFactory : IAdapterFactory
	{
		private readonly IDictionary<Type, object> _adapters = new ConcurrentDictionary<Type, object>();
		
		public void RegisterAdapter<TAdapter>(TAdapter adapter)
		{
			if(!ValidateAdapter(typeof(TAdapter)))
			{
				throw new InvalidOperationException($"Adapter {typeof(TAdapter)} does not implement {typeof(IGenericAdapter<>)}");
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

		public TResult Adapt<TResult, TAdaptee>(TAdaptee adaptee) where TResult : class
		{
			foreach (var adapter in _adapters)
			{
				var genericTypeArguments = adapter.Key.GenericTypeArguments;
				if (genericTypeArguments.Any() && genericTypeArguments.First() == typeof(TResult))
				{
					var genericAdapter = (IGenericAdapter<TResult>)adapter.Value;
					return genericAdapter.Adapt(adaptee);
				}
			}
			throw new InvalidOperationException($"Adapter for type {typeof(TResult)} is not defined.");
		}

		private static bool ValidateAdapter(Type adapterType)
		{
			if (adapterType.IsInterface)
			{
				return adapterType.IsGenericType && adapterType.GetGenericTypeDefinition() == typeof(IGenericAdapter<>);
			}
			return adapterType.GetInterfaces().Any(iface => iface.IsGenericType && iface.GetGenericTypeDefinition() == typeof(IGenericAdapter<>));
		}
	}
}
