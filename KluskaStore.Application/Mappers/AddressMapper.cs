using KluskaStore.Application.Features.Addresses;
using KluskaStore.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace KluskaStore.Application.Mappers;

[Mapper]
public static partial class AddressMapper
{
    public static partial AddressResponse ToResponse(this Address address);
}
