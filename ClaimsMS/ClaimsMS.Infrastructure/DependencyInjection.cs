using ClaimsMS.Common.Primitives;
using ClaimsMS.Core.Database;
using ClaimsMS.Infrastructure.Database.Context.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;


namespace ClaimsMS.Infrastructure
{

    [ExcludeFromCodeCoverage]

    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddPersistence(configuration);
            return services;
        }

        private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("PostgresSQLConnection");
                options.UseNpgsql(connectionString);
            });
            services.AddScoped<IApplicationDbContext>(product => product.GetRequiredService<ApplicationDbContext>()!);
            services.AddScoped<IUnitOfWork>(product => product.GetRequiredService<ApplicationDbContext>()!);

          //  services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
           // services.AddScoped<IProductRepository, ProductRepository>();
           // services.AddScoped<ICategoryRepository, CategoryRepository>();

            return services;
        }
    }
}