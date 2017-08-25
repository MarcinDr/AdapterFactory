namespace GenericAdapterFactory.Interfaces
{
	public interface IAdapterFactory
	{
		void RegisterAdapter<TAdapter>(TAdapter adapter);
		TAdapter ResolveAdapter<TAdapter>();
		bool RemoveAdapter<TAdapter>();

		TResult Adapt<TResult, TAdaptee>(TAdaptee adaptee) where TResult : class;
	}
}