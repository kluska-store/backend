using KluskaStore.Application.Interfaces.Stores;
using KluskaStore.Domain.Entities;
using KluskaStore.Domain.Repositories;
using KluskaStore.Domain.Shared;
using KluskaStore.Domain.ValueObjects;

namespace KluskaStore.Application.Features.Stores.CreateStore;

public class CreateStoreHandler(IUnitOfWork uow) :
  Handler<CreateStoreCommand, Result<StoreResponse>>(uow),
  ICreateStoreHandler {
  public override async Task<Result<StoreResponse>> Handle(
    CreateStoreCommand request,
    CancellationToken cancellationToken = default
  ) {
    var cnpjResult = Cnpj.Create(request.Cnpj);
    var emailResult = Email.Create(request.Email);

    List<string> errors = [];
    if (!cnpjResult.IsFailure) errors.AddRange(cnpjResult.Errors);
    if (!emailResult.IsFailure) errors.AddRange(emailResult.Errors);

    var storeResult = Store.Create(cnpjResult.Value, request.Name, emailResult.Value, request.Password);
    if (storeResult.IsFailure) errors.AddRange(storeResult.Errors);
    
    if (errors.Count > 0) return Result<StoreResponse>.Failure(errors);

    var store = storeResult.Value;
    await UoW.Stores.AddAsync(store);
    await UoW.SaveChangesAsync();

    return Result<StoreResponse>.Success(new StoreResponse(store.Id, store.Cnpj.Value, store.Name, store.Email.Value,
      store.IsActive));
  }
}