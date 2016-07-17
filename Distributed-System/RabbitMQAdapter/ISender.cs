namespace RabbitMQAdapter
{
    public interface ISender
    {
        void Send(byte[] buffer);

        void AddHeader(string key, string value);
    }
}