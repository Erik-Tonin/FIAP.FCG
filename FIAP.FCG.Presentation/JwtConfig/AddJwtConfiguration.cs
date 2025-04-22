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
                    options.Authority = "http://localhost:8080/realms/fcg-realm"; // URL do Keycloak
                    options.Audience = "account"; // O "aud" do token
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = "http://localhost:8080/realms/fcg-realm",
                        ValidAudience = "account"
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = context =>
                        {
                            // Adiciona as roles do token ao usuário autenticado
                            var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
                            var roles = claimsIdentity?.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

                            // Logar as roles encontradas para depuração
                            Log.Information("Roles found in token: {Roles}", string.Join(", ", roles));

                            return Task.CompletedTask;
                        }
                    };
                });
        }

        public static void UseAuthConfiguration(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}