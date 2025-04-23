using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Security.Claims;

namespace FIAP.FCG.Presentation.JwtConfig
{
    public static class JwTokemConfig
    {
        public static void AddJwtConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = "http://localhost:8080/realms/fcg-realm";
                    options.Audience = "account";
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = "http://localhost:8080/realms/fcg-realm",
                        ValidAudience = "account",
                        RoleClaimType = "roles" // Ou "realm_access.roles" dependendo de como você configurou
                    };
                    options.RequireHttpsMetadata = false; // Desabilita a exigência de HTTPS
                });
        }

        public static void UseAuthConfiguration(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}