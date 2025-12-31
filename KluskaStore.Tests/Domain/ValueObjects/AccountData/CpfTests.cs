using KluskaStore.Domain.Errors.ValueObjects;
using KluskaStore.Domain.ValueObjects.AccountData;

namespace KluskaStore.Tests.Domain.ValueObjects.AccountData;

public class CpfTests
{
    [Theory]
    [InlineData("01234567890")]
    [InlineData("11122233396")]
    public void GivenVoCreation_WhenInitialDataIsValid_ThenCreatesVo(string cpf)
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
    public void GivenVoCreation_WhenCpfIsInvalid_ThenReturnsInvalidVoResult(string cpf)
    {
        var result = Cpf.Create(cpf);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(CpfErrors.InvalidCpf.Code);
    }
}
