
using clothes_backend.Inteface;
using clothes_backend.Middleware;
using clothes_backend.Models;
using clothes_backend.Repository;
using clothes_backend.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace clothes_backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            //error handle
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(op =>
            {
                op.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "clothes-backend", Version = "v1" });
                op.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                op.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });

            });
           
            builder.Services.AddMemoryCache();
            builder.Services.AddSingleton<ICacheService, CacheService>();
            //builder.Services.AddLogging();
            //Database
            builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Database")));
            //JWT
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(option => 
                {
                    option.TokenValidationParameters = new TokenValidationParameters
                    {
                        RequireExpirationTime = false,
                        //
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,                
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
                    };

                });

            //
            builder.Services.AddHttpContextAccessor();
            //
            builder.Services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));          
            builder.Services.AddScoped<ProductRepository>();
            builder.Services.AddScoped<ProductOptionImageRepository>();
            builder.Services.AddScoped<ProductVariantsRepository>();
            builder.Services.AddScoped<UserRepositpory>();
            builder.Services.AddScoped<AuthService>();
            //builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddScoped<CartRepository>();
            
            builder.Services.AddAutoMapper(typeof(Program));
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment() || app.Environment.IsProduction() || app.Environment.IsStaging())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Images")),
                RequestPath = "/Images"
            });

            app.UseHttpsRedirection();
            //
           
            app.UseAuthentication();
            app.UseAuthorization();
            //MIDDLEWARE
            //check token
            app.Use(async (context, next) =>
            {              
                var access_token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                if (!string.IsNullOrEmpty(access_token))
                {
                    //Check expired
                    var check_token = new JwtSecurityTokenHandler().ReadToken(access_token) as JwtSecurityToken;
                    if(check_token.ValidTo <= DateTime.UtcNow)
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsync($"Token is expired {check_token.ValidTo}");
                        return;
                    }
                    using (var scope = app.Services.CreateScope())
                    {
                        var _db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                        var isblockToken = await _db.blacklist_token.AnyAsync(x => x.token == access_token);
                        if (isblockToken)
                        {
                            //block request
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;                        
                            await context.Response.WriteAsync("Token is revoked");
                            Console.WriteLine("Token is revoked");
                            return;
                        }
                    }
                }
                //save current user
                await next(context);
            });
            //app.UseMiddleware<UserMiddleware>();
            //store user
            app.Use(async (context, next) =>
            {
                if (context.Request.Path.StartsWithSegments("/api/cart"))
                {
                    if (context.User.Identity.IsAuthenticated)
                    {
                        context.Items["IsUser"] = context.User.FindFirst(ClaimTypes.Name)?.Value;
                    }
                }              
                await next();
            });          
            app.MapControllers();
            app.Run();
        }
    }
}
