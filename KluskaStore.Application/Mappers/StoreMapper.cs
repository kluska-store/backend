using KluskaStore.Application.Features.Addresses;
using KluskaStore.Application.Features.Stores;
using KluskaStore.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace KluskaStore.Application.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class StoreMapper
{
    public static partial StoreResponse ToResponse(this Store s);

    private static AddressResponse MapAddressToResponse(Address address) => address.ToResponse();
}
