using clothes_backend.Interfaces;
using clothes_backend.Interfaces.Repository;
using clothes_backend.Interfaces.Service;
using clothes_backend.Repositories;
using clothes_backend.Repository;
using clothes_backend.Service;
using clothes_backend.Services;


namespace clothes_backend.Heplers.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public static class AppServiceMiddleware
    {
       public static IServiceCollection AddApplicationServices(this IServiceCollection services)
       {          
           services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));         
           services.AddScoped<ProductOptionImageRepository>();
           services.AddScoped<ProductVariantsRepository>();
           services.AddScoped<UserRepositpory>();
           services.AddScoped<VerifyHandleService>();
           services.AddScoped<CartRepositoryOld>();
           services.AddScoped<IUserContextService, UserContextService>();
           services.AddScoped<OrderRepositoryOld>();
           //mail           
           services.AddScoped<IBackgroundJobService, BackgroundJobService>();
           services.AddScoped<EmailService>(); 
           //
           services.AddScoped<IProductRepository, ProductRepository>();
           services.AddScoped<IProductService, ProductService>();
           services.AddScoped<IUserRepository, UserRepositpory>();
           services.AddScoped<IUserService, UserService>();
           services.AddScoped<RemoveFileService>();
           services.AddScoped<IImageRepository, ImageRepository>();
           services.AddScoped<IImageService, ImageSerivce>();
           services.AddScoped<IImageHandler, ImageHandler>();
           services.AddScoped<IVariantRepository, VariantRepository>();
           services.AddScoped<IVariantService, VariantService>();
           //
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IOrderReposiotry, OrderRepository>();
            services.AddScoped<IOrderService, OrderService>();
            //automapper
            return services;
       }
    }      
}
