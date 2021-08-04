using Library.Domain.Interfaces;
using Library.Domain.Models;
using System.Threading.Tasks;

namespace Library.Application
{
    public class ProducerServices : IProducerServices
    {
        private readonly IRabbitServices _services;
        public ProducerServices(IRabbitServices services)
        {
            _services = services;
        }

        public async Task SendCustomerToQueue(string dado)
        {
            await _services.SendMessageAsync(dado, Contexto.IntegracaoClientes);
        }
        public async Task SendBookToQueue(string dado)
        {
            await _services.SendMessageAsync(dado, Contexto.Integracaolivros);
        }
    }
}
