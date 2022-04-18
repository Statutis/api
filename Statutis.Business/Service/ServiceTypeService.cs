using Statutis.Core.Interfaces.Business.Service;
using Statutis.Core.Interfaces.DbRepository.Service;
using Statutis.Entity.Service;

namespace Statutis.Business;

public class ServiceTypeService : IServiceTypeService
{
	private IServiceTypeRepository _repository;

	public ServiceTypeService(IServiceTypeRepository repository)
	{
		_repository = repository;
	}

	public Task<List<ServiceType>> GetAll()
	{
		return _repository.GetAll();
	}

	public Task<ServiceType?> Get(string name)
	{
		return _repository.Get(name);
	}

	public Task<ServiceType> Insert(ServiceType serviceType)
	{
		return _repository.Insert(serviceType);
	}

	public Task<ServiceType> Update(ServiceType serviceType)
	{
		return _repository.Update(serviceType);
	}

	public void Delete(ServiceType serviceType)
	{
		_repository.Delete(serviceType);
	}
}