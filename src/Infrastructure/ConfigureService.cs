using Infrastructure.Data;
using Infrastructure.Enitites;
using Infrastructure.Interface;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared.Constants;

namespace Infrastructure;
//ref: https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-9.0&tabs=visual-studio
public static class ConfigureService
{
    public static IServiceCollection AddInfrastructureService(this IServiceCollection services, AppSettings configuration)
    {
        services.AddDbContext<ApplicationDbContext>(p=>p.UseSqlServer(configuration.ConnectionStrings.DefaultConnection));

        services.AddIdentity<User, Role>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IProductImageRepository, ProductImageRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductVariantRepository, ProductVariantRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IForgotPasswordRepository, ForgotPasswordRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
