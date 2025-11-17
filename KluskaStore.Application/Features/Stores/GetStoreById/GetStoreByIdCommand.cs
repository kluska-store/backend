using KluskaStore.Domain.Shared;
using MediatR;

namespace KluskaStore.Application.Features.Stores.GetStoreById;

public record GetStoreByIdCommand (Guid StoreId) : IRequest<Result<StoreResponse>>;