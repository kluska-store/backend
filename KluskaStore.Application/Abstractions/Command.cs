using KluskaStore.Domain.Shared;
using MediatR;

namespace KluskaStore.Application.Abstractions;

public abstract record Command<T> : IRequest<Result<T>>;