using KluskaStore.Application.Interfaces;
using Crypt = BCrypt.Net.BCrypt;

namespace KluskaStore.Infrastructure.Security;

public class BCryptHasher : IPasswordHasher
{
    public string Hash(string password) => Crypt.HashPassword(password, Crypt.GenerateSalt());
    public bool Verify(string password, string hash) => Crypt.Verify(password, hash);
}