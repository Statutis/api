namespace Statutis.Core.Interfaces.Business;

public interface IPasswordHash
{
    string Hash(string password);

    bool Verify(string passwordHashed, string plainPassword);
}