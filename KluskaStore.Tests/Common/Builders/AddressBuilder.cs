using KluskaStore.Domain.ValueObjects.AccountData.Address;

namespace KluskaStore.Tests.Common.Builders;

public static class AddressBuilder
{
    private static Address Generate(
        string? country = null,
        string? state = null,
        string? city = null,
        string? street = null
    ) => new(
        country ?? "country",
        state ?? "state",
        city ?? "city",
        street ?? "street",
        1,
        PostalCodeBuilder.Valid(),
        ""
    );

    public static Address Valid() => Generate();
    public static Address NoCountry() => Generate(country: "");
    public static Address NoState() => Generate(state: "");
    public static Address NoCity() => Generate(city: "");
    public static Address NoStreet() => Generate(street: "");
}
