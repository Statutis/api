using Statutis.Entity.Service;

namespace Statutis.Core.Interfaces.Business.Service;

public interface IServiceService
{
	//Get
	Task<List<Entity.Service.Service>> GetAll();

	public Task<List<T>> GetAll<T>() where T : Entity.Service.Service;

	Task<List<Entity.Service.Service>> GetByHost(string host);

	Task<List<Entity.Service.Service>> GetByServiceType(ServiceType serviceType);

	Task<List<Entity.Service.Service>> Get(string name);

	Task<Entity.Service.Service?> Get(Guid guid);

	Task<T?> GetByClass<T>(Guid guid) where T : Entity.Service.Service;

	//insert
	Task<Entity.Service.Service> Insert(Entity.Service.Service service);

	//Update
	Task<T> Update<T>(T service) where T : Entity.Service.Service;

	//Delete
	Task Delete(Entity.Service.Service delete);
}