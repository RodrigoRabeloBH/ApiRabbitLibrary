using Library.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Library.RabbitMQ.Extension
{
    public static class RabbitServicesExtensions
    {
        public static IServiceCollection AddRabbitServicesExtensions(this IServiceCollection services)
        {
            services.AddTransient<IRabbitServices, RabbitServices>();

            return services;
        }
    }
}
