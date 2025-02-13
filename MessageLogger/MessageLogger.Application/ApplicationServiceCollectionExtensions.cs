using MessageLogger.Application.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace MessageLogger.Application;
public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<ILogRepository, LogRepository>();
        return services;
    }
}