using KluskaStore.Domain.Shared;
using KluskaStore.Domain.Interfaces;

namespace KluskaStore.Domain.ValueObjects;

public class Address : IValueObject
{
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
        List<string> errors = [];
        if (string.IsNullOrWhiteSpace(country)) errors.Add("Country must not be null");
        if (string.IsNullOrWhiteSpace(state)) errors.Add("State must not be null");
        if (string.IsNullOrWhiteSpace(city)) errors.Add("City must not be null");
        if (string.IsNullOrWhiteSpace(street)) errors.Add("Street must not be null");

        if (string.IsNullOrWhiteSpace(complement)) complement = null;

        return errors.Count > 0
            ? Result<Address>.Failure(errors)
            : Result<Address>.Success(new Address(country, state, city, street, number, postalCode, complement));
    }
}