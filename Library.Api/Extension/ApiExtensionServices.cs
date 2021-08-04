using Library.Application.Extension;
using Library.RabbitMQ.Extension;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Api.Extension
{
    public static class ApiExtensionServices
    {
        public static IServiceCollection AddApiExtensionServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRabbitServicesExtensions();
            services.AddApplicationExtension();
            services.AddSwaggerDocumentation();

            return services;
        }
    }
}
