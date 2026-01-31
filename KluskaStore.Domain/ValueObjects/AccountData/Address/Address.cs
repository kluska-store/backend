using static KluskaStore.Domain.Errors.ValueObjects.AddressErrors;

namespace KluskaStore.Domain.ValueObjects.AccountData.Address;

public sealed record Address
{
    // TODO: attribute default value for non-nullable fields
    private Address() { }

    internal Address(
        string country,
        string state,
        string city,
        string street,
        uint number,
        PostalCode postalCode,
        string? complement
    )
    {
        Country = country;
        State = state;
        City = city;
        Street = street;
        Number = number;
        PostalCode = postalCode;
        Complement = complement;
    }

    public string Country { get; }
    public string State { get; }
    public string City { get; }
    public string Street { get; }
    public uint Number { get; }
    public PostalCode PostalCode { get; }
    public string? Complement { get; }

    public static Result<Address> Create(
        string country,
        string state,
        string city,
        string street,
        uint number,
        PostalCode postalCode,
        string? complement = null
    )
    {
        Error? error = null;
        if (string.IsNullOrWhiteSpace(country)) error = MissingCountry;
        if (string.IsNullOrWhiteSpace(state)) error = MissingState;
        if (string.IsNullOrWhiteSpace(city)) error = MissingCity;
        if (string.IsNullOrWhiteSpace(street)) error = MissingStreet;

        if (string.IsNullOrWhiteSpace(complement)) complement = null;

        return error is not null
            ? Result<Address>.Failure(error)
            : Result<Address>.Success(new Address(country, state, city, street, number, postalCode, complement));
    }
}
