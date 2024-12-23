﻿namespace Infrastructure;

using Infrastructure.Identity;
using Infrastructure.Persistence;
using Infrastructure.Tenancy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddMultiTenancyServices(configuration)
            .AddPersistenceServices(configuration)
            .AddIdentityServices();
    }
}
