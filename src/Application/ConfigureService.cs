using Application.Common.Interface;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ConfigureService
{
    public static IServiceCollection AddApplicationService(this IServiceCollection services)
    {
        services.AddTransient<IAuthService, AuthService>();
        services.AddTransient<IEmailService, EmailService>();
        services.AddTransient<IFileService, LocalStorageService>();
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

