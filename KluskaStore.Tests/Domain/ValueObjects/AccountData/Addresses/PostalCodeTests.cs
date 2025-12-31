using KluskaStore.Domain.Errors.ValueObjects;
using KluskaStore.Domain.ValueObjects.AccountData.Address;

namespace KluskaStore.Tests.Domain.ValueObjects.AccountData.Addresses;

public class PostalCodeTests
{
    [Fact]
    public void GivenValidPostalCode_ThenCreatesVo()
    {
        var postalCodeStr = "02732020";
        var result = PostalCode.Create(postalCodeStr);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Value.Should().Be(postalCodeStr);
    }

    [Theory]
    [InlineData("0273202")]
    [InlineData("027320202")]
    [InlineData("")]
    [InlineData("0273202O")]
    public void GivenInvalidPostalCode_ThenReturnsFailure(string postalCodeStr)
    {
        var result = PostalCode.Create(postalCodeStr);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(PostalCodeErrors.InvalidPostalCode.Code);
    }
}
