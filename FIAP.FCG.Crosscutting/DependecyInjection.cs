using FIAP.FCG.Application.Contracts.IApplicationService;
using FIAP.FCG.Application.Contracts.IRepositories;
using FIAP.FCG.Application.Implementations;
using FIAP.FCG.Infra.Context;
using FIAP.FCG.Infra.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FIAP.FCG.Crosscutting
{
    public static class DependecyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MicroServiceContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("Connection"))
                    .EnableSensitiveDataLogging();
            });

            services.AddCors(options =>
            {
                options.AddPolicy("Total",
                    builder =>
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader());
            });

            // ApplicationService
            services.AddScoped<IUserProfileApplicationService, UserProfileApplicationService>();

            // Repositories
            services.AddScoped<IUserProfileRepository, UserProfileRepositorie>();

            return services;
        }
    }
}