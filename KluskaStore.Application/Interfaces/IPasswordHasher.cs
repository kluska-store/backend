namespace KluskaStore.Application.Interfaces;

public interface IPasswordHasher
{
    public string Hash(string passowrd);
    public bool Verify(string password, string hash);
}