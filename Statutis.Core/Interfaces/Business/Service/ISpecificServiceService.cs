namespace Statutis.Core.Interfaces.Business.Service;

public interface ISpecificServiceService : IServiceService
{
    bool IsSupported(Entity.Service.Service service);
}