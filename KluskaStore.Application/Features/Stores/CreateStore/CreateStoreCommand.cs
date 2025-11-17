using KluskaStore.Application.Abstractions;

namespace KluskaStore.Application.Features.Stores.CreateStore;

public record CreateStoreCommand(string Cnpj, string Name, string Email, string Password) : Command<StoreResponse>;