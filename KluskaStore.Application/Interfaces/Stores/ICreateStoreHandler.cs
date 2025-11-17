using KluskaStore.Application.Features.Stores;
using KluskaStore.Application.Features.Stores.CreateStore;
using KluskaStore.Domain.Shared;
using MediatR;

namespace KluskaStore.Application.Interfaces.Stores;

public interface ICreateStoreHandler : IRequestHandler<CreateStoreCommand, Result<StoreResponse>>;