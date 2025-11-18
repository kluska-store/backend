using KluskaStore.Application.Abstractions;
using KluskaStore.Application.Mappers;
using KluskaStore.Domain.Repositories;
using KluskaStore.Domain.Shared;

namespace KluskaStore.Application.Features.Stores.GetStoreByCnpj;

public class GetStoreByCnpjHandler(IUnitOfWork uow) : Handler<CnpjQuery, Result<StoreResponse>>(uow)
{
    public override async Task<Result<StoreResponse>> Handle(
        CnpjQuery request,
        CancellationToken cancellationToken = default
    )
    {
        var store = await UoW.Stores.GetByCnpjAsync(request.Cnpj);
        if (store is null) return Result<StoreResponse>.Failure($"Store with CNPJ {request.Cnpj} not found");

        return Result<StoreResponse>.Success(store.ToResponse());
    }
}