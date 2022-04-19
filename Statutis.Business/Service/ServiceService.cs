using Statutis.Core.Interfaces.Business.Service;
using Statutis.Core.Interfaces.DbRepository.Service;
using Statutis.Entity.Service;

namespace Statutis.Business;

public class ServiceService : IServiceService
{
	private IServiceRepository _repository;

	public ServiceService(IServiceRepository repository)
	{
		_repository = repository;
	}

	public Task<List<Service>> GetAll()
	{
		return _repository.GetAll();
	}

	public Task<List<Service>> GetByHost(string host)
	{
		return _repository.GetByHost(host);
	}

	public Task<List<Service>> GetByServiceType(ServiceType serviceType)
	{
		return _repository.GetByServiceType(serviceType);
	}

	public Task<List<Service>> Get(string name)
	{
		return _repository.Get(name);
	}

	public Task<Service> Get(Guid guid)
	{
		return _repository.Get(guid);
	}

	public Task<Service> Insert(Service service)
	{
		return _repository.Insert(service);
	}

	public Task<Service> Update(Service service)
	{
		return _repository.Update(service);
	}

	public async Task Delete(Service delete)
	{
		await _repository.Delete(delete);
	}
}