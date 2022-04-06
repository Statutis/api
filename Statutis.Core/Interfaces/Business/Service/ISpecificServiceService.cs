namespace Statutis.Core.Interfaces.Business.Service;

public interface ISpecificServiceService : IServiceService
{
    Task<bool> IsSupported(Entity.Service.Service service);
}