using Isopoh.Cryptography.Argon2;
using Statutis.Core.Interfaces.Business;

namespace Statutis.Business;

public class PasswordHash : IPasswordHash
{
    public string Hash(string password)
    {
        return Argon2.Hash(password);
    }

    public bool Verify(string passwordHashed, string plainPassword)
    {
        return Argon2.Verify(passwordHashed, plainPassword);
    }
}