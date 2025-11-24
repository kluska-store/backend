using FluentAssertions;
using KluskaStore.Domain.Entities;
using KluskaStore.Domain.ValueObjects;

namespace KluskaStore.Tests.Domain.Entities;

public class StoreTests
{
    [Fact]
    public void GivenValidStore_ThenCreatesEntity()
    {
        var name = "test store";
        var password = "12345678";
        var cnpj = Cnpj.Create("00000000000000", skipVerifierDigitsValidation: true).Value;
        var email = Email.Create("test@store.com").Value;
        var postalCode = PostalCode.Create("00000000").Value;
        var address = Address.Create("country", "state", "city", "street", 0, postalCode, "complement").Value;
        var result = Store.Create(cnpj, name, email, password, address);

        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        result.Value.Should().BeAssignableTo<Store>();
        result.Value.Name.Should().Be(name);
        result.Value.PasswordHash.Should().Be(password);
        result.Value.Cnpj.Should().Be(cnpj);
        result.Value.Address.Should().Be(address);
    }

    [Fact]
    public void GivenInvalidStore_ThenReturnsFailure()
    {
        var result = Store.Create(null!, "", null!, "", null!);

        result.IsFailure.Should().BeTrue();
        result.Errors.Count.Should().Be(2);
        result.Value.Should().BeNull();
    }
}
