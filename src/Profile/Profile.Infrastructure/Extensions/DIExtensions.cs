using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Profile.Core.Domain.RepositoryContracts;
using Profile.Infrastructure.Data;
using Profile.Infrastructure.Repositories;

namespace Profile.Infrastructure.Extensions;

public static class DIExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(config.GetSection("ConnectionStrings:DefaultConnection").Value);
        });
        
        services.AddScoped<IProfileRepository, ProfileRepository>();
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = config["IdentityServiceUrl"];
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters.ValidateAudience = false;
                // options.TokenValidationParameters.NameClaimType = "username";
            });

        return services;
    }
}