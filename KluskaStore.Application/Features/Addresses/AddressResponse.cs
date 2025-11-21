namespace KluskaStore.Application.Features.Addresses;

public record AddressResponse(
    uint id,
    string country,
    string state,
    string city,
    string street,
    uint number,
    string postalCode,
    string? complement
);
