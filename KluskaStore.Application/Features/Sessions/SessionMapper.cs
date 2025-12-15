using KluskaStore.Application.Features.Sessions.GetSessionByToken;
using KluskaStore.Domain.Entities.Accounts;
using KluskaStore.Domain.ValueObjects;
using Riok.Mapperly.Abstractions;

namespace KluskaStore.Application.Features.Sessions;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class SessionMapper
{
    [MapProperty(nameof(Session.Id), nameof(GetSessionByTokenResponse.Token))]
    [MapProperty(nameof(Session.Owner.OwnerId), nameof(GetSessionByTokenResponse.OwnerId))]
    [MapProperty(nameof(Session.Owner), nameof(GetSessionByTokenResponse.OwnerType))]
    public partial GetSessionByTokenResponse ToResponse(Session session);

    private static SessionOwnerType Map(SessionOwner owner) => owner.OwnerType switch
    {
        SessionOwner.OwnerTypeEnum.User => SessionOwnerType.User,
        SessionOwner.OwnerTypeEnum.Store => SessionOwnerType.Store,
        _ => throw new ArgumentOutOfRangeException(nameof(owner), "Owner type not mapped")
    };
}
