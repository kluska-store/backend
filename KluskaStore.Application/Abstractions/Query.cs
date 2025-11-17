using KluskaStore.Domain.Shared;
using MediatR;

namespace KluskaStore.Application.Abstractions;

public abstract record Query<TResp> : IRequest<Result<TResp>>;