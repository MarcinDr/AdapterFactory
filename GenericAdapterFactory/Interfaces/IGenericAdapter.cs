using System;

namespace GenericAdapterFactory.Interfaces
{
	public interface IGenericAdapter<TAdapter>
	{
		void RegisterAdaptFunction<TAdaptee>(Func<TAdaptee, TAdapter> adaptFunc);
		TAdapter Adapt<TAdaptee>(TAdaptee adaptee);
		bool RemoveAdaptFunction<TAdaptee>();
	}
}