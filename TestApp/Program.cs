using System;
using GenericAdapterFactory;
using GenericAdapterFactory.Interfaces;

namespace TestApp
{
	public class Program
	{
		private static readonly ApiService ApiService = new ApiService();
		
		public static void Main(string[] args)
		{
			IAdapterFactory adaptersFactory = new AdapterFactory();
			
			IGenericAdapter<IPerson> genericAdapter = new GenericAdapter<IPerson>();
			genericAdapter.RegisterAdaptFunction<ExternalPersonEntity>(entity =>
			{
				var splittedName = entity.FullName.Split(' ');
				return new Person(splittedName[0], splittedName[1]);
			});
			genericAdapter.RegisterAdaptFunction<DatabasePersonEntity>(entity => new Person(entity.Name, entity.Surname));
			genericAdapter.RegisterAdaptFunction<ServicePersonEntity>(entity => new Person(entity.Name, entity.Surname));
			
			adaptersFactory.RegisterAdapter(genericAdapter);

			var testData = PrepareTestData();

			ApiService.RegisterPerson(adaptersFactory.ResolveAdapter<IGenericAdapter<IPerson>>().Adapt(testData.Ext));
			ApiService.RegisterPerson(adaptersFactory.ResolveAdapter<IGenericAdapter<IPerson>>().Adapt(testData.Db));
			ApiService.RegisterPerson(adaptersFactory.ResolveAdapter<IGenericAdapter<IPerson>>().Adapt(testData.Serv));
			ApiService.RegisterPerson(adaptersFactory.Adapt<IPerson, ExternalPersonEntity>(testData.Ext));

			Console.ReadLine();
		}

		private static (ExternalPersonEntity Ext, DatabasePersonEntity Db, ServicePersonEntity Serv) PrepareTestData()
		{
			var external = new ExternalPersonEntity("John Doe");
			var database = new DatabasePersonEntity("John", "Doe");
			var service = new ServicePersonEntity("John", "Doe");

			return (external, database, service);
		}
	}

	public class PersonSimpleAdapter
	{
		public IPerson AdaptExternalPerson(ExternalPersonEntity externalPersonEntity)
		{
			var splittedName = externalPersonEntity.FullName.Split(' ');
			return new Person(splittedName[0], splittedName[1]);
		}

		public IPerson AdaptDatabasePerson(DatabasePersonEntity databasePersonEntity)
		{
			return new Person(databasePersonEntity.Name, databasePersonEntity.Surname);
		}

		public IPerson AdaptServicePerson(ServicePersonEntity servicePersonEntity)
		{
			return new Person(servicePersonEntity.Name, servicePersonEntity.Surname);
		}
	}

	public class ApiService
	{
		public void RegisterPerson(IPerson person)
		{
			Console.WriteLine($"{person.Name} {person.Surname}");
		}
	}

	public interface IPerson
	{
		string Name { get; }
		string Surname { get; }
	}

	public class Person : IPerson
	{
		public string Name { get; }
		public string Surname { get; }

		public Person(string name, string surname)
		{
			Name = name;
			Surname = surname;
		}
	}

	public class ExternalPersonEntity
	{
		public string FullName { get; }
		
		public ExternalPersonEntity(string fullName)
		{
			FullName = fullName;
		}
	}
	
	public class DatabasePersonEntity
	{
		public string Name { get; }
		public string Surname { get; }

		public DatabasePersonEntity(string name, string surname)
		{
			Name = name;
			Surname = surname;
		}
	}
	
	public class ServicePersonEntity
	{
		public string Name { get; }
		public string Surname { get; }

		public ServicePersonEntity(string name, string surname)
		{
			Name = name;
			Surname = surname;
		}
	}
}
