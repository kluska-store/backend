using KluskaStore.Application.Abstractions;
using KluskaStore.Application.Mappers;
using KluskaStore.Domain.Entities;
using KluskaStore.Domain.Repositories;
using KluskaStore.Domain.Shared;
using KluskaStore.Domain.ValueObjects;

namespace KluskaStore.Application.Features.Stores.CreateStore;

public class CreateStoreWithAddressHandler(IUnitOfWork uow)
    : Handler<CreateStoreWithAddressCommand, Result<StoreResponse>>(uow)
{
    public override async Task<Result<StoreResponse>> Handle(
        CreateStoreWithAddressCommand request,
        CancellationToken cancellationToken = default
    )
    {
        var addressData = request.Address;

        var cnpjResult = Cnpj.Create(request.Cnpj);
        var emailResult = Email.Create(request.Email);
        var postalCodeResult = PostalCode.Create(addressData.postalCode);

        List<string> errors = [];
        if (cnpjResult.IsFailure) errors.AddRange(cnpjResult.Errors);
        if (emailResult.IsFailure) errors.AddRange(emailResult.Errors);
        if (postalCodeResult.IsFailure) errors.AddRange(postalCodeResult.Errors);

        var addressResult = Address.Create(
            addressData.country,
            addressData.state,
            addressData.city,
            addressData.street,
            addressData.number,
            postalCodeResult.Value,
            addressData.complement
        );

        if (addressResult.IsFailure) errors.AddRange(addressResult.Errors);

        var storeResult = Store.Create(
            cnpjResult.Value,
            request.Name,
            emailResult.Value,
            request.Password,
            addressResult.Value
        );

        if (storeResult.IsFailure) errors.AddRange(storeResult.Errors);

        if (errors.Count > 0) return Result<StoreResponse>.Failure(errors);

        var store = storeResult.Value;
        var address = addressResult.Value;

        if (!string.IsNullOrWhiteSpace(request.PictureUrl)) store.ChangePicture(request.PictureUrl);

        try
        {
            await UoW.BeginTransactionAsync();
            await UoW.Stores.AddAsync(store);
            await UoW.Addresses.AddAsync(address);
            await UoW.SaveChangesAsync();
            await UoW.CommitTransactionAsync();
        }
        catch (Exception)
        {
            await UoW.RollbackTransactionAsync();
            throw;
        }

        return Result<StoreResponse>.Success(store.ToResponse());
    }
}