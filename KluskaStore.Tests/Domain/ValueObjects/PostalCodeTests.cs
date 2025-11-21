using FluentAssertions;
using KluskaStore.Domain.ValueObjects;

namespace KluskaStore.Tests.Domain.ValueObjects;

public class PostalCodeTests
{
    [Fact]
    public void GivenValidPostalCode_ThenCreatesVo()
    {
        var postalCodeStr = "02732020";
        var result = PostalCode.Create(postalCodeStr);

        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        result.Value.Should().BeAssignableTo<PostalCode>();
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
        result.Errors.Should().NotBeNullOrEmpty();
        result.Value.Should().BeNull();
    }
}
