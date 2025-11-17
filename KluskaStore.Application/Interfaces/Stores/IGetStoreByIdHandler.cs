using KluskaStore.Application.Abstractions;
using KluskaStore.Application.Features.Stores;
using KluskaStore.Domain.Shared;
using MediatR;

namespace KluskaStore.Application.Interfaces.Stores;

public interface IGetStoreByIdHandler : IRequestHandler<GuidQuery<StoreResponse>, Result<StoreResponse>>;