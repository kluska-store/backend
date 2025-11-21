using KluskaStore.Domain.Shared;
using KluskaStore.Domain.ValueObjects;

namespace KluskaStore.Domain.Entities;

public class Address : Entity<uint>
{
    private Address() { }

    private Address(
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

    public string Country { get; private set; }
    public string State { get; private set; }
    public string City { get; private set; }
    public string Street { get; private set; }
    public uint Number { get; private set; }
    public PostalCode PostalCode { get; private set; }
    public string? Complement;

    public static Result<Address> Create(
        string country,
        string state,
        string city,
        string street,
        uint number,
        PostalCode postalCode,
        string? complement
    )
    {
        List<string> errors = [];
        if (string.IsNullOrWhiteSpace(country)) errors.Add("Country must not be null");
        if (string.IsNullOrWhiteSpace(state)) errors.Add("State must not be null");
        if (string.IsNullOrWhiteSpace(city)) errors.Add("City must not be null");
        if (string.IsNullOrWhiteSpace(street)) errors.Add("Street must not be null");

        return errors.Count > 0
            ? Result<Address>.Failure(errors)
            : Result<Address>.Success(new Address(country, state, city, street, number, postalCode, complement));
    }
}
