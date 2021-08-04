using Library.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.Domain.Interfaces
{
    public interface IRabbitServices
    {
        Task<bool> SendMessageAsync(string messagem, Contexto contexto);
        IEnumerable<Message> ReceiverMessage(string queueName);
        void ConfirmDelivery(IEnumerable<Message> messages);
    }
}
