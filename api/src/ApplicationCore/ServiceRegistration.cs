using FluentValidation;
using Homemap.ApplicationCore.Interfaces.Services;
using Homemap.ApplicationCore.Mappings;
using Homemap.ApplicationCore.Services;
using Homemap.ApplicationCore.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace Homemap.ApplicationCore
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddMappers(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(ProjectProfile).Assembly);

            return services;
        }

        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<ProjectDtoValidator>();

            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IReceiverService, ReceiverService>();
            services.AddScoped<IDeviceService, DeviceService>();

            return services;
        }
    }
}
