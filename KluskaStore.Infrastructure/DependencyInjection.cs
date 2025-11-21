using KluskaStore.Application.Interfaces;
using KluskaStore.Domain.Repositories;
using KluskaStore.Infrastructure.Data;
using KluskaStore.Infrastructure.Repositories;
using KluskaStore.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace KluskaStore.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IPasswordHasher, BCryptHasher>();
        return services;
    }
}
