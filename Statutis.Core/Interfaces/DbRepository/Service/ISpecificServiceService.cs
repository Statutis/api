namespace Statutis.Core.Interfaces.DbRepository.Service;

public interface ISpecificServiceService : IServiceService
{
    bool IsSupported(Entity.Service.Service service);
}