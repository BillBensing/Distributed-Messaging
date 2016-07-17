using System.ComponentModel;

namespace RabbitMQAdapter
{
    public interface IListener
    {
        void Start();

        void Stop();

        void ConfigureModel();

        void ConfigureConsumer();

        event PropertyChangedEventHandler PropertyChanged;

        byte[] ReceivedMessage();
    }
}