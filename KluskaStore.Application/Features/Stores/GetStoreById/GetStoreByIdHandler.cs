using KluskaStore.Application.Abstractions;
using KluskaStore.Domain.Repositories;
using KluskaStore.Domain.Shared;

namespace KluskaStore.Application.Features.Stores.GetStoreById;

public class GetStoreByIdHandler(IUnitOfWork uow) : Handler<GuidQuery<StoreResponse>, Result<StoreResponse>>(uow)
{
    public override async Task<Result<StoreResponse>> Handle(
        GuidQuery<StoreResponse> request,
        CancellationToken cancellationToken = default
    )
    {
        var store = await UoW.Stores.GetByIdAsync(request.Guid);
        if (store is null) return Result<StoreResponse>.Failure($"Store with id {request.Guid} not found");

        var response = new StoreResponse(store.Id, store.Cnpj.Value, store.Name, store.Email.Value, store.IsActive);
        return Result<StoreResponse>.Success(response);
    }
}