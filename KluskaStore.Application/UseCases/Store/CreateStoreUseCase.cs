using KluskaStore.Application.DTOs.Store;
using KluskaStore.Application.Exceptions;
using KluskaStore.Application.Interfaces;
using KluskaStore.Domain.Repositories;
using KluskaStore.Domain.ValueObjects;

namespace KluskaStore.Application.UseCases.Store;

public class CreateStoreUseCase(IUnitOfWork uow)
  : UseCase<CreateStoreRequest, StoreResponse>(uow), ICreateStoreUseCase {
  public override async Task<StoreResponse> ExecuteAsync(CreateStoreRequest request) {
    Domain.Entities.Store store;
    var cnpjResult = Cnpj.New(request.Cnpj);
    var emailResult = Email.New(request.Email);

    if (!cnpjResult.IsValid) throw new BadRequestException(cnpjResult.Error!);
    if (!emailResult.IsValid) throw new BadRequestException(emailResult.Error!);

    store = new Domain.Entities.Store(cnpjResult.Vo!, request.Name, emailResult.Vo!, request.Password);

    await UoW.Stores.AddAsync(store);
    await UoW.SaveChangesAsync();

    return new StoreResponse(store.Id, store.Cnpj.Value, store.Name, store.Email.Value, store.IsActive);
  }
}