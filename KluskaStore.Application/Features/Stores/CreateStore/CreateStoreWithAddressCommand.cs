using KluskaStore.Application.Abstractions;
using KluskaStore.Application.Features.Addresses;

namespace KluskaStore.Application.Features.Stores.CreateStore;

public record CreateStoreWithAddressCommand(
    string Cnpj,
    string Name,
    string Email,
    string Password,
    string? PictureUrl,
    AddressData Address
) : Command<StoreResponse>;
