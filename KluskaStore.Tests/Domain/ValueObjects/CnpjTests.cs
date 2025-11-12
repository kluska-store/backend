using FluentAssertions;
using KluskaStore.Domain.ValueObjects;

namespace KluskaStore.Tests.Domain.ValueObjects;

public class CnpjTests {
  [Theory]
  [InlineData("04658924000130")]
  [InlineData("10619284000152")]
  public void GivenValidCnpj_WhenNotSkippingVerifierDigitsValidation_ThenCreatesVo(string cnpj) {
    var result = Cnpj.New(cnpj);

    result.IsValid.Should().BeTrue();
    result.Error.Should().BeNull();
    result.Vo.Should().BeAssignableTo<Cnpj>();
    result.Vo.Value.Should().Be(cnpj);
  }

  [Theory]
  [InlineData("0465892400013000")]
  [InlineData("04658924000132")]
  [InlineData("04658924w00013")]
  [InlineData("00000000000000")]
  [InlineData("04658924000")]
  public void GivenInvalidCnpj_WhenNotSkippingVerifierDigistValidation_ThenReturnsInvalidVoResult(string cnpj) {
    var result = Cnpj.New(cnpj);

    result.IsValid.Should().BeFalse();
    result.Error.Should().NotBeNullOrWhiteSpace();
    result.Vo.Should().BeNull();
  }
}