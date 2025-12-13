namespace KluskaStore.Application.Interfaces;

public interface IPasswordHasher
{
    string Hash(string passowrd);
    bool Verify(string password, string hash);
}
