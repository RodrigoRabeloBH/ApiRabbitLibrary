using Library.Domain.Models;
using System.Collections.Generic;

namespace Library.Domain.Interfaces
{
    public interface IConsumerServices
    {
        IEnumerable<Message> GetMessages(string queueName);
        void ConfirmDeliveryMessage(IEnumerable<Message> messages);
    }
}
