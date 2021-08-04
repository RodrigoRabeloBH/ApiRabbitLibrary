namespace Library.Domain.Models
{
    public class Message
    {
        public ulong DeliveryTag { get; set; }
        public string Content { get; set; }
        public bool Executed { get; set; }
    }
}
