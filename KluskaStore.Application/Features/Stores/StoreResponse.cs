namespace KluskaStore.Application.Features.Stores;

public record StoreResponse(Guid Id, string Cnpj, string Name, string Email, bool IsActive );