using FIAP.FCG.Application.Contracts.IApplicationService;
using FIAP.FCG.Domain.Contracts.IRepositories;
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
            // Corrigido para adicionar apenas uma configuração do DbContext
            services.AddDbContext<MicroServiceContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("FiapConnection")));

            // Configuração de CORS
            services.AddCors(options =>
            {
                options.AddPolicy("Total",
                    builder =>
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader());
            });

            // Registro dos serviços de aplicação
            services.AddScoped<IUserProfileApplicationService, UserProfileApplicationService>();
            services.AddScoped<IGameApplicationService, GameApplicationService>();

            // Registro dos repositórios
            services.AddScoped<IUserProfileRepository, UserProfileRepositorie>();
            services.AddScoped<IGameRepository, GameRepositorie>();

            return services;
        }
    }
}
