using KluskaStore.Domain.ValueObjects;

namespace KluskaStore.Domain.Entities;

public class Store : Entity<Guid> {
  public Cnpj Cnpj { get; private set; }
  public string Name { get; private set; }
  public string? PictureUrl { get; private set; }
  public Email Email { get; private set; }
  public bool IsActive { get; private set; }
  public string PasswordHash { get; private set; }

  private Store() { }

  public Store(Cnpj cnpj, string name, Email email, string passwordHash) : base(Guid.Empty) {
    if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name must not be empty", nameof(name));
    if (string.IsNullOrWhiteSpace(passwordHash))
      throw new ArgumentException("Password must not be empty", nameof(passwordHash));

    Cnpj = cnpj;
    Name = name;
    Email = email;
    IsActive = true;
    PasswordHash = passwordHash;
  }

  public void ChangeName(string name) {
    if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name must not be empty", nameof(name));
    Name = name;
  }

  public void ChangeEmail(Email email) => Email = email;
  public void ChangePicture(string? url) => PictureUrl = url;

  public void ChangePasswordHash(string hash) {
    if (string.IsNullOrWhiteSpace(hash)) throw new ArgumentException("Password must not be empty", nameof(hash));
    PasswordHash = hash;
  }

  public void Activate() => IsActive = true;
  public void Deactivate() => IsActive = false;
}