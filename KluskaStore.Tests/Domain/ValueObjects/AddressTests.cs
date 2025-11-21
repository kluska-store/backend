using FluentAssertions;
using KluskaStore.Domain.ValueObjects;

namespace KluskaStore.Tests.Domain.ValueObjects;

public class AddressTests
{
    private const string PostalCodeStr = "12345678";
    private const string Country = "tomorrowland";
    private const string State = "magic district";
    private const string City = "oz city";
    private const string Street = "Glinda avenue";
    private const uint Number = 2;

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void GivenValidAddress_WhenWithoutComplement_ThenCreatesVo(string? complement)
    {
        var postalCode = PostalCode.Create(PostalCodeStr).Value;
        var result = Address.Create(Country, State, City, Street, Number, postalCode, complement);

        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        result.Value.Should().BeAssignableTo<Address>();
        result.Value.Country.Should().Be(Country);
        result.Value.State.Should().Be(State);
        result.Value.City.Should().Be(City);
        result.Value.Street.Should().Be(Street);
        result.Value.Number.Should().Be(Number);
        result.Value.PostalCode.Should().Be(postalCode);
        result.Value.Complement.Should().BeNull();
    }

    [Fact]
    public void GivenValidAddress_WhenWithComplement_ThenCreatesVo()
    {
        const string complement = "next to shame tower";
        var postalCode = PostalCode.Create(PostalCodeStr).Value;
        var result = Address.Create(Country, State, City, Street, Number, postalCode, complement);

        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        result.Value.Should().BeAssignableTo<Address>();
        result.Value.Country.Should().Be(Country);
        result.Value.State.Should().Be(State);
        result.Value.City.Should().Be(City);
        result.Value.Street.Should().Be(Street);
        result.Value.Number.Should().Be(Number);
        result.Value.PostalCode.Should().Be(postalCode);
        result.Value.Complement.Should().Be(complement);
    }

    [Fact]
    public void GivenInvalidAddress_WhenAllFieldsAreWrong_ThenReturnsFailure()
    {
        var postalCode = PostalCode.Create(PostalCodeStr).Value;
        var result = Address.Create("", "", "", "", 0, postalCode, "");

        result.IsFailure.Should().BeTrue();
        result.Value.Should().BeNull();
        result.Errors.Count.Should().Be(4);
    }
}
