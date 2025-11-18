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
    }
    
    public string Country { get; private set; }
    public string State { get; private set; }
    public string City { get; private set; }
    public string Street { get; private set; }
    public uint Number { get; private set; }
    public PostalCode PostalCode { get; set; }
}