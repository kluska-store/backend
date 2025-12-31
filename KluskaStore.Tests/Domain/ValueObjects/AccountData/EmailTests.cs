using KluskaStore.Domain.Errors.ValueObjects;
using Email = KluskaStore.Domain.ValueObjects.AccountData.Email;

namespace KluskaStore.Tests.Domain.ValueObjects.AccountData;

public class EmailTests
{
    [Theory]
    [InlineData("example@email.com")]
    [InlineData("example@email.com.br")]
    [InlineData("bigger.example@email.com")]
    [InlineData("numeric55.example_123@email.com")]
    public void GivenValidEmail_ThenCreatesVo(string email)
    {
        var result = Email.Create(email);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Value.Should().Be(email);
    }

    [Theory]
    [InlineData("example@.com")]
    [InlineData("example@email")]
    [InlineData(".example@email.com")]
    [InlineData("example@email.com.")]
    [InlineData("exampleemail.com")]
    [InlineData("example@email@com")]
    public void GivenInvalidEmail_ThenCreatesInvalidVoResult(string email)
    {
        var result = Email.Create(email);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(EmailErrors.InvalidEmail.Code);
    }
}
