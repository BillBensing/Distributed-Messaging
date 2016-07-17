using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace RabbitMQAdapter
{
    public class Listener : INotifyPropertyChanged
    {
        public string Queue { get; private set; }
        public string Exchange { get; private set; }
        public byte[] ReceivedMessage { get; private set; }
        public Dictionary<string, string> ReceivedHeaders { get; private set; }
        public IModel Model { get; private set; }

        private delegate void OnReceiveMessage(string message);

        public event PropertyChangedEventHandler PropertyChanged;

        public bool Enabled { get; private set; }
        private QueueingBasicConsumer _consumer;

        public Listener(IModel Model, string Queue, string Exchange)
        {
            this.Queue = Queue;
            this.Exchange = Exchange;
            this.Model = Model;
            this.ReceivedHeaders = new Dictionary<string, string>();

            _consumer = new QueueingBasicConsumer(this.Model);
            this.ConfigureModel();
            this.ConfigureConsumer();
        }

        public void Start()
        {
            this.Enabled = true;

            Console.WriteLine("\n*-----------------------------------------------------*");
            Console.WriteLine(string.Format("Listening on queue {0}", this.Queue));
            Console.WriteLine("*-----------------------------------------------------*\n");

            while (Enabled)
            {
                var deliveryArgs = (BasicDeliverEventArgs)_consumer.Queue.Dequeue();
                this.ReceivedMessage = deliveryArgs.Body;
                PopulateReceivedHeaders(deliveryArgs);
                this.Model.BasicAck(deliveryArgs.DeliveryTag, false);
                NotifyPropertyChanged("ReceivedMessage");
            }
        }

        public void Stop()
        {
            this.Enabled = false;
        }

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public virtual void ConfigureModel()
        {
            this.Model.BasicQos(0, 1, false);
        }

        public virtual void ConfigureConsumer()
        {
            this.Model.BasicConsume(this.Queue, false, _consumer);
        }

        private void PopulateReceivedHeaders(BasicDeliverEventArgs deliveryArgs)
        {
            this.ReceivedHeaders.Clear();
            if (deliveryArgs.BasicProperties.Headers != null)
            {
                MarshalHeaders(deliveryArgs);
            }
        }

        private void MarshalHeaders(BasicDeliverEventArgs deliveryArgs)
        {
            foreach (var key in deliveryArgs.BasicProperties.Headers.Keys)
            {
                string value = Encoding.Default.GetString((byte[])deliveryArgs.BasicProperties.Headers[key]);
                this.ReceivedHeaders.Add(key, value);
            }
        }
    }
}