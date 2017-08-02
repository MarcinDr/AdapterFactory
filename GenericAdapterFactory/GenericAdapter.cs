using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using GenericAdapterFactory.Interfaces;

namespace GenericAdapterFactory
{
	public class GenericAdapter<TAdapter> : IGenericAdapter<TAdapter>
	{
		private readonly IDictionary<Type, object> _adaptFunctions = new ConcurrentDictionary<Type, object>();
		
		public void RegisterAdaptFunction<TAdaptee>(Func<TAdaptee, TAdapter> adaptFunc)
		{
			if (_adaptFunctions.ContainsKey(typeof(TAdaptee)))
			{
				throw new InvalidOperationException($"Adapt function for {typeof(TAdaptee)} is already defined");
			}
			_adaptFunctions.Add(typeof(TAdaptee), adaptFunc);
		}

		public TAdapter Adapt<TAdaptee>(TAdaptee adaptee)
		{
			object value;
			if (!_adaptFunctions.TryGetValue(typeof(TAdaptee), out value))
			{
				throw new InvalidOperationException($"Adapt function for {typeof(TAdaptee)} is not defined");
			}
			var func = (Func<TAdaptee, TAdapter>)value;
			return func(adaptee);
		}

		public bool RemoveAdaptFunction<TAdaptee>()
		{
			return _adaptFunctions.Remove(typeof(TAdaptee));
		}
	}
}
