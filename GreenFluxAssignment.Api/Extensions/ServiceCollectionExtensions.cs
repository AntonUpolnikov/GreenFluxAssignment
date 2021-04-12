using GreenFluxAssignment.Domain.Interfaces.Repository;
using GreenFluxAssignment.Domain.Interfaces.Services;
using GreenFluxAssignment.Domain.Services;
using GreenFluxAssignment.Persistence.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace GreenFluxAssignment.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddScoped<IConnectorService, ConnectorService>();
            services.AddScoped<IChargeStationService, ChargeStationService>();
            services.AddScoped<IGroupService, GroupService>();
            return services;
        }
    }
}
