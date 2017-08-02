namespace GenericAdapterFactory.Interfaces
{
	public interface IAdapterFactory
	{
		void RegisterAdapter<TAdapter>(TAdapter adapter);
		TAdapter ResolveAdapter<TAdapter>();
		bool RemoveAdapter<TAdapter>();
	}
}
