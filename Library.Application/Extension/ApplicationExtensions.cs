using Library.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Application.Extension
{
    public static class ApplicationExtensions
    {
        public static IServiceCollection AddApplicationExtension(this IServiceCollection services)
        {
            services.AddTransient<IProducerServices, ProducerServices>();
            services.AddTransient<IConsumerServices, ConsumerServices>();
            return services;
        }
    }
}
