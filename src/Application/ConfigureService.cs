using Application.Common.Interface;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Shared.Constants;

namespace Application;

public static class ConfigureService
{
    public static IServiceCollection AddApplicationService(this IServiceCollection services, AppSettings appSettings)
    {
        services.AddTransient<IAuthService, AuthService>();
        services.AddTransient<IEmailService, EmailService>();

        if (appSettings.FileStorageSettings.LocalStorage)
        {
            services.AddSingleton<IFileService, LocalStorageService>();
        }    

        services.AddTransient<IOrderService, OrderService>();
        services.AddTransient<IProductImageService, ProductImageService>();
        services.AddTransient<IProductService, ProductService>();
        services.AddTransient<IProductVariantService, ProductVariantService>();
        services.AddTransient<IUserService, UserService>();

        services.AddTransient<IProductVariantFilterService, ProductVariantFilterService>();

        services.AddAutoMapper(typeof(ApplicationRoot).Assembly);

        return services;
    }
}

