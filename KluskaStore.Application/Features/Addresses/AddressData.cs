namespace KluskaStore.Application.Features.Addresses;

public record AddressData(
    string country,
    string state,
    string city,
    string street,
    uint number,
    string postalCode,
    string? complement
);