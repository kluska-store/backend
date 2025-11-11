using KluskaStore.Application.DTOs.Store;
using KluskaStore.Application.Interfaces;
using KluskaStore.Domain.Repositories;
using KluskaStore.Domain.ValueObjects;

namespace KluskaStore.Application.UseCases.Store;

public class CreateStoreUseCase(IUnitOfWork uow)
  : UseCase<CreateStoreRequest, StoreResponse>(uow), ICreateStoreUseCase {
  
  public override async Task<StoreResponse> ExecuteAsync(CreateStoreRequest request) {
    var cnpj = new Cnpj(request.Cnpj);
    var email = new Email(request.Email);

    var store = new Domain.Entities.Store(cnpj, request.Name, email, request.Password);

    await UoW.Stores.AddAsync(store);
    await UoW.SaveChangesAsync();

    return new StoreResponse(store.Id, store.Cnpj.Value, store.Name, store.Email.Value, store.IsActive);
  }
}