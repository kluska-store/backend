using FluentAssertions;
using KluskaStore.Domain.ValueObjects;

namespace KluskaStore.Tests.Domain.ValueObjects;

public class PhoneTests
{
    [Theory]
    [InlineData("1", "212", "55555-1234")]
    [InlineData("55", "11", "94567-1234")]
    [InlineData("351", "21", "95555-6789")]
    [InlineData("55", "31", "3456-7890")]
    public void GivenValidPhone_ThenCreatesVo(string ddi, string ddd, string phoneNumber)
    {
        var phone = $"+{ddi} ({ddd}) {phoneNumber}";
        var result = Phone.Create(phone);

        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        result.Value.Should().BeAssignableTo<Phone>();
        result.Value.FullPhone.Should().Be(phone);
        result.Value.Ddi.Should().Be(ddi);
        result.Value.Ddd.Should().Be(ddd);
        result.Value.PhoneNumber.Should().Be(phoneNumber);
    }

    [Theory]
    [InlineData("", "212", "55555-1234")]
    [InlineData("1111", "212", "55555-1234")]
    [InlineData("1w", "212", "55555-1234")]
    [InlineData("1", "", "55555-1234")]
    [InlineData("1", "2122", "55555-1234")]
    [InlineData("1", "21w", "55555-1234")]
    [InlineData("1", "212", "")]
    [InlineData("1", "212", "55555-12345")]
    [InlineData("1", "212", "555551234")]
    [InlineData("1", "212", "5555w1234")]
    public void GivenInvalidPhone_ThenReturnsFailure(string ddi, string ddd, string phoneNumber)
    {
        var phone = $"+{ddi} ({ddd}) {phoneNumber}";
        var result = Phone.Create(phone);

        result.IsFailure.Should().BeTrue();
        result.Errors.Should().NotBeNullOrEmpty();
        result.Value.Should().BeNull();
    }
}
