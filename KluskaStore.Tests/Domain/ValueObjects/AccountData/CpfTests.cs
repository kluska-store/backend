using KluskaStore.Domain.Errors.ValueObjects;
using KluskaStore.Domain.ValueObjects.AccountData;

namespace KluskaStore.Tests.Domain.ValueObjects.AccountData;

public class CpfTests
{
    [Theory]
    [InlineData("01234567890")]
    [InlineData("11122233396")]
    public void GivenValidCpf_WhenNotSkippingVerifierDigitsValidation_ThenCreatesVo(string cpf)
    {
        var result = Cpf.Create(cpf);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Value.Should().Be(cpf);
    }

    [Theory]
    [InlineData("012345678900")]
    [InlineData("01234567891")]
    [InlineData("0123w567890")]
    [InlineData("00000000000")]
    [InlineData("0123456789")]
    public void GivenInvalidCnpj_WhenNotSkippingVerifierDigistValidation_ThenReturnsInvalidVoResult(string cpf)
    {
        var result = Cpf.Create(cpf);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(CpfErrors.InvalidCpf.Code);
    }

    [Theory]
    [InlineData("00000000000")]
    [InlineData("01234567891")]
    public void GivenValidCnpj_WhenSkippingVerifierDigitsValidation_TheCreatesVo(string cpf)
    {
        var result = Cpf.Create(cpf, true);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Value.Should().Be(cpf);
    }
}
