using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ClaimsMS;
using ClaimsMS.Infrastructure.Database.Context.Postgres;
using ClaimsMS.Core.Database;
using ClaimsMS.Core.Repositories.Resolutions;
using ClaimsMS.Infrastructure.Repositories.Resolution;
using ClaimsMS.Infrastructure.Repositories.Claims;
using ClaimsMS.Core.Repositories.Claims;
using PaymentMS.Infrastructure.Services.User;
using ClaimsMS.Application.Resolution.Handler.Command;
using ClaimsMS.Application.Claim.Handler.Command;


namespace ClaimsMS
{
    [ExcludeFromCodeCoverage]
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGenWithAuth(configuration);
            services.KeycloakConfiguration(configuration);

            //* Sin los Scope no funciona!!
            //services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
            services.AddScoped<IResolutionRepository, ResolutionRepository>();
            services.AddScoped<IResolutionRepositoryMongo, ResolutionRepositoryMongo>();
            services.AddScoped<IClaimRepository, ClaimRepository>();
            services.AddScoped<IClaimDeliveryRepository, ClaimDeliveryRepository>();
            services.AddScoped<IClaimRepositoryMongo, ClaimRepositoryMongo>();
            services.AddScoped<IClaimDeliveryRepositoryMongo, ClaimDeliveryRepositoryMongo>();
            services.AddScoped<ApplicationDbContext>();
            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
            //Registro de handlers 
            services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(CreateResolutionCommandHandler).Assembly));
            services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(CreateClaimCommandHandler).Assembly));
            services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(CreateClaimDeliveryCommandHandler).Assembly));
            services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(UpdateStatusClaimDeliveryCommandHandler).Assembly));
            services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(UpdateStatusCommadnHandler).Assembly));
            //services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(UpdateProductCommandHandler).Assembly));

          //  services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(GetNameProductQueryHandler).Assembly));
         //   services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(GetAvailableProductsQueryHandler).Assembly));
           // services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(GetProductQueryHandler).Assembly));
          //  services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(GetAllProductQueryHandler).Assembly));
            //services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(GetFilteredProductsQueryHandler).Assembly));
            services.AddHttpClient<UserService>(
                client =>
                {
                    client.BaseAddress = new Uri("https://localhost:18084");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                }
            );
            return services;
        }
    }
}