using FluentAssertions;
using KluskaStore.Domain.Entities;
using KluskaStore.Domain.ValueObjects;

namespace KluskaStore.Tests.Domain.Entities;

public class StoreTests
{
    private readonly Store _sut = new(
        new Cnpj("cnpj"),
        "name",
        new Email("email"),
        "password",
        new Address(
            "country",
            "state",
            "city",
            "street",
            0,
            new PostalCode("postal code"),
            "complement"
        ),
        new Phone("phone")
    );

    [Fact]
    public void GivenEntityCreation_WhenDataIsValid_ThenCreatesStore()
    {
        var name = "test store";
        var password = "12345678";
        var cnpj = Cnpj.Create("00000000000000", skipVerifierDigitsValidation: true).Value;
        var email = Email.Create("test@store.com").Value;
        var postalCode = PostalCode.Create("00000000").Value;
        var address = Address.Create("country", "state", "city", "street", 0, postalCode, "complement").Value;
        List<string> phonesStr = ["+00 (00) 00000-0001", "+00 (00) 00000-0002", "+00 (00) 00000-0003"];
        var phones = phonesStr.Select(Phone.Create).Select(r => r.Value).ToList();

        var result = Store.Create(cnpj, name, email, password, address, phones);

        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        result.Value.Should().BeAssignableTo<Store>();
        result.Value.Name.Should().Be(name);
        result.Value.PasswordHash.Should().Be(password);
        result.Value.Cnpj.Should().Be(cnpj);
        result.Value.Address.Should().Be(address);
        result.Value.Phones.Should().BeEquivalentTo(phones);
    }

    [Fact]
    public void GivenEntityCreation_WhenDataIsInvalid_ThenReturnsFailure()
    {
        var result = Store.Create(null!, "", null!, "", null!);

        result.IsFailure.Should().BeTrue();
        result.Errors.Count.Should().Be(2);
        result.Value.Should().BeNull();
    }

    [Fact]
    public void GivenNameChange_WhenValidNewName_ThenChangesName()
    {
        var newName = "new name";
        var result = _sut.ChangeName("new name");

        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        result.Value.Should().BeSameAs(_sut);
        _sut.Name.Should().Be(newName);
    }

    [Fact]
    public void GivenNameChange_WhenInvalidNewName_ThenReturnsFailure()
    {
        var result = _sut.ChangeName("");

        result.IsFailure.Should().BeTrue();
        result.Errors.Should().NotBeNullOrEmpty();
        result.Value.Should().BeNull();
    }

    [Fact]
    public void GivenPictureUrlChange_ThenChangesPictureUrl()
    {
        var newUrl = "https://example.com";
        _sut.ChangePicture(newUrl);

        _sut.PictureUrl.Should().Be(newUrl);
    }

    [Fact]
    public void GivenPasswordChange_WhenValidNewPassword_ThenChangesPassword()
    {
        var newPassword = "password123";
        var result = _sut.ChangePasswordHash(newPassword);

        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        result.Value.Should().BeSameAs(_sut);
        _sut.PasswordHash.Should().Be(newPassword);
    }

    [Fact]
    public void GivenPasswordChange_WhenInvalidNewPassword_ThenReturnsFailure()
    {
        var result = _sut.ChangePasswordHash("");

        result.IsFailure.Should().BeTrue();
        result.Errors.Should().NotBeNullOrEmpty();
        result.Value.Should().BeNull();
    }

    [Fact]
    public void GivenAddressChange_ThenChangesAddress()
    {
        var newAddress = new Address("country 2", "state 2", "city 2", "street 2", 2, new PostalCode("postal code 2"),
            null);
        _sut.ChangeAddress(newAddress);

        _sut.Address.Should().Be(newAddress);
    }

    [Fact]
    public void GivenDeactivationAndReactivation_ThenDeactivatesAndReactivates()
    {
        _sut.IsActive.Should().BeTrue();

        _sut.Deactivate();
        _sut.IsActive.Should().BeFalse();

        _sut.Activate();
        _sut.IsActive.Should().BeTrue();
    }

    [Fact]
    public void GivenPhoneAdditionAndRemoval_ThenAddsAndRemovesPhone()
    {
        var newPhone = new Phone("phone 2");
        _sut.AddPhones(newPhone);
        _sut.Phones[^1].Should().Be(newPhone);

        _sut.RemovePhone(newPhone);
        _sut.Phones.Should().NotContain(newPhone);
    }
}
