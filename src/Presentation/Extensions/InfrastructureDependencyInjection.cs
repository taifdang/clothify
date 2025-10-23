using Application.Common.Interface;
using Application.Services;
using Shared.Constants;

namespace Api.Extensions;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IWebHostEnvironment env, IConfiguration configuration)
    {
        var settings = configuration.GetSection("AppSettings").Get<AppSettings>() ?? new AppSettings();

        if (string.IsNullOrEmpty(settings.BaseUrl))
        {
            settings.BaseUrl = "";
        }

        services.AddSingleton(settings);
        services.AddScoped<IFileService, LocalStorageService>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<LocalStorageService>>();
            return new LocalStorageService(env.WebRootPath, settings, logger);
        });
        return services;
    }
}
