using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GreenFluxAssignment.Api.Mapper;
using GreenFluxAssignment.Api.Extensions;
using GreenFluxAssignment.Api.Filters;
using GreenFluxAssignment.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using GreenFluxAssignment.Persistence.Repository;

namespace GreenFluxAssignment.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add<GlobalExceptionFilter>();
            });
            services
                .AddDomainServices()
                .AddDatabaseServices(options =>
                {
                    options.UseSqlServer(Configuration.GetValue<string>("ConnectionStrings:SqlServer"));
                })
                .AddAutoMapper(typeof(GroupsProfile))
                .AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app)
        {
            ApplyMigrations(app);
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Assignment API");
            });
        }

        private void ApplyMigrations(IApplicationBuilder builder)
        {
            using var serviceScope = builder.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<GroupContext>();
            context.Database.Migrate();
        }
    }
}
