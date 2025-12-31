using KluskaStore.Domain.Errors.ValueObjects;
using KluskaStore.Domain.ValueObjects.AccountData.Address;

namespace KluskaStore.Tests.Domain.ValueObjects.AccountData.Addresses;

public class AddressTests
{
    private readonly Address _sut = new(
        "tomorrowland",
        "magic district",
        "oz city",
        "Glinda avenue",
        2,
        new PostalCode("12345678"),
        "next to shame tower"
    );

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void GivenValidAddress_WhenWithoutComplement_ThenCreatesVo(string? complement)
    {
        var result = Address.Create(
            _sut.Country,
            _sut.State,
            _sut.City,
            _sut.Street,
            _sut.Number,
            _sut.PostalCode,
            complement
        );

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Country.Should().Be(_sut.Country);
        result.Value.State.Should().Be(_sut.State);
        result.Value.City.Should().Be(_sut.City);
        result.Value.Street.Should().Be(_sut.Street);
        result.Value.Number.Should().Be(_sut.Number);
        result.Value.PostalCode.Should().Be(_sut.PostalCode);
        result.Value.Complement.Should().BeNull();
    }

    [Fact]
    public void GivenValidAddress_WhenWithComplement_ThenCreatesVo()
    {
        var result = Address.Create(
            _sut.Country,
            _sut.State,
            _sut.City,
            _sut.Street,
            _sut.Number,
            _sut.PostalCode,
            _sut.Complement
        );

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Country.Should().Be(_sut.Country);
        result.Value.State.Should().Be(_sut.State);
        result.Value.City.Should().Be(_sut.City);
        result.Value.Street.Should().Be(_sut.Street);
        result.Value.Number.Should().Be(_sut.Number);
        result.Value.PostalCode.Should().Be(_sut.PostalCode);
        result.Value.Complement.Should().Be(_sut.Complement);
    }

    [Fact]
    public void GivenVoCreation_WhenNoCountryIsProvided_ThenReturnsFailure()
    {
        var result = Address.Create(
            "",
            _sut.State,
            _sut.City,
            _sut.Street,
            _sut.Number,
            _sut.PostalCode,
            _sut.Complement
        );

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(AddressErrors.MissingCountry.Code);
    }

    [Fact]
    public void GivenVoCreation_WhenNoStateIsProvided_ThenReturnsFailure()
    {
        var result = Address.Create(
            _sut.Country,
            "",
            _sut.City,
            _sut.Street,
            _sut.Number,
            _sut.PostalCode,
            _sut.Complement
        );

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(AddressErrors.MissingState.Code);
    }

    [Fact]
    public void GivenVoCreation_WhenNoCityIsProvided_ThenReturnsFailure()
    {
        var result = Address.Create(
            _sut.Country,
            _sut.State,
            "",
            _sut.Street,
            _sut.Number,
            _sut.PostalCode,
            _sut.Complement
        );

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(AddressErrors.MissingCity.Code);
    }

    [Fact]
    public void GivenVoCreation_WhenNoStreetIsProvided_ThenReturnsFailure()
    {
        var result = Address.Create(
            _sut.Country,
            _sut.State,
            _sut.City,
            "",
            _sut.Number,
            _sut.PostalCode,
            _sut.Complement
        );

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(AddressErrors.MissingStreet.Code);
    }
}
