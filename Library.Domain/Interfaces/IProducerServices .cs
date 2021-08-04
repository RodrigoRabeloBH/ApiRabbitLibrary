using System.Threading.Tasks;

namespace Library.Domain.Interfaces
{
    public interface IProducerServices
    {
        Task SendCustomerToQueue(string dado);
        Task SendBookToQueue(string dado);
    }
}
