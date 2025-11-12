using FluentAssertions;
using KluskaStore.Domain.ValueObjects;

namespace KluskaStore.Tests.Domain.ValueObjects;

public class EmailTests {
  [Theory]
  [InlineData("example@email.com")]
  [InlineData("example@email.com.br")]
  [InlineData("bigger.example@email.com")]
  [InlineData("numeric55.example_123@email.com")]
  public void GivenValidEmail_ThenCreatesVo(string email) {
    var result = Email.New(email);

    result.IsValid.Should().BeTrue();
    result.Error.Should().BeNull();
    result.Vo.Should().BeAssignableTo<Email>();
    result.Vo.Value.Should().Be(email);
  }

  [Theory]
  [InlineData("example@.com")]
  [InlineData("example@email")]
  [InlineData(".example@email.com")]
  [InlineData("example@email.com.")]
  [InlineData("exampleemail.com")]
  [InlineData("example@email@com")]
  public void GivenInvalidEmail_ThenCreatesInvalidVoResult(string email) {
    var result = Email.New(email);

    result.IsValid.Should().BeFalse();
    result.Error.Should().NotBeNullOrWhiteSpace();
    result.Vo.Should().BeNull();
  }
}