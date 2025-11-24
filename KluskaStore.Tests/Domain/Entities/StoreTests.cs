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
        new Phone("phone"), new Phone("phone 2"), new Phone("phone 3")
    );

    [Fact]
    public void GivenEntityCreation_WhenDataIsValid_ThenCreatesStore()
    {
        var result = Store.Create(_sut.Cnpj, _sut.Name, _sut.Email, _sut.PasswordHash, _sut.Address, _sut.Phones);

        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        result.Value.Should().BeAssignableTo<Store>();
        result.Value.IsActive.Should().BeTrue();
        result.Value.Cnpj.Should().Be(_sut.Cnpj);
        result.Value.Name.Should().Be(_sut.Name);
        result.Value.Email.Should().Be(_sut.Email);
        result.Value.PasswordHash.Should().Be(_sut.PasswordHash);
        result.Value.Address.Should().Be(_sut.Address);
        result.Value.Phones.Should().BeEquivalentTo(_sut.Phones);
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
        var newPhone = new Phone("phone extra");
        _sut.AddPhones(newPhone);
        _sut.Phones[^1].Should().Be(newPhone);

        _sut.RemovePhone(newPhone);
        _sut.Phones.Should().NotContain(newPhone);
    }
}
