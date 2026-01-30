using KluskaStore.Application.Abstractions;

namespace KluskaStore.Tests.Common.Mocks.SessionTokenGenerator;

public static class SessionTokenGeneratorMockExtensions
{
    public static void SetupGenerateNewTokenReturns(this Mock<ISessionTokenGenerator> mock, string token) => mock
        .Setup(gen => gen.New())
        .Returns(token);

    public static void VerifyGenerateNewTokenCalled(this Mock<ISessionTokenGenerator> mock, Func<Times> times)
        => mock.Verify(gen => gen.New(), times);
}
