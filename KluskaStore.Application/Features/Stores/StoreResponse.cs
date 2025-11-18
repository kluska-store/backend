namespace KluskaStore.Application.Features.Stores;

// TODO: implement DTO mapper
public record StoreResponse(Guid Id, string Cnpj, string Name, string Email, bool IsActive);