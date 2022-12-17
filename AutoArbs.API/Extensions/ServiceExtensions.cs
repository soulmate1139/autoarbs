using AutoArbs.Application.Interfaces;
using AutoArbs.Infrastructure.Repositories;
using AutoArbs.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AutoArbs.API.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services) =>
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
            builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
        });

        public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration) =>
        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration.GetSection("Jwt")["Key"])),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });
    
        public static void ConfigureAuthenticationService(this IServiceCollection services, IConfiguration configuration) => services.AddSingleton<IJwtAuthenticationManager>(new JwtAuthenticationManager(configuration.GetSection("Jwt")["Key"]));
        public static void ConfigureRepositoryManager(this IServiceCollection services) => services.AddScoped<IRepositoryManager, RepositoryManager>();
        public static void ConfigureServiceManager(this IServiceCollection services) => services.AddScoped<IServiceManager, ServiceManager>();
        public static void ConfigureDatabaseContext(this IServiceCollection services, IConfiguration configuration) => services.AddDbContext<RepositoryContext>(opts => opts.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
    }
}