using System;
using GreenFluxAssignment.Domain.Interfaces.Repository;
using GreenFluxAssignment.Persistence.Interfaces;
using GreenFluxAssignment.Persistence.Mapper;
using GreenFluxAssignment.Persistence.Repository;
using GreenFluxAssignment.Persistence.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GreenFluxAssignment.Persistence.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabaseServices(
            this IServiceCollection services,
            Action<DbContextOptionsBuilder> configure)
        {
            services.AddAutoMapper(typeof(GroupsProfile).Assembly);
            services.AddSingleton<IEntityConverter, EntityConverter>();
            services.AddDbContext<GroupContext>(configure);
            services.AddScoped<IGroupRepository, GroupRepository>();

            return services;
        }
    }
}
