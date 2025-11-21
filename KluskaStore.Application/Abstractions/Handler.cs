using KluskaStore.Domain.Repositories;
using MediatR;

namespace KluskaStore.Application.Abstractions;

public abstract class Handler<TReq, TResp>(IUnitOfWork uow) : IRequestHandler<TReq, TResp> where TReq : IRequest<TResp>
{
    protected readonly IUnitOfWork UoW = uow;

    public abstract Task<TResp> Handle(TReq request, CancellationToken cancellationToken = default);
}
