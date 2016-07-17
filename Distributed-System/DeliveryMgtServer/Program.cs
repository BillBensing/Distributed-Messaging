using MessageSerializer;
using RabbitMQ.Client;
using RabbitMQAdapter;
using RabbitMQAdapter.Configuration;
using System;
using System.ComponentModel;

namespace DeliveryMgtServer
{
    internal class Program
    {
        private static Listener _receiver;
        private static IModel _model;

        private static void Main(string[] args)
        {
            try
            {
                _model = new ConnectionAdapter(DemoConnection.HOST, DemoConnection.USER, DemoConnection.PW).GetModel();
                _receiver = new Listener(_model, "DeliveryMgt.NewDelivery", "");
                _receiver.PropertyChanged += Listen;
                _receiver.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\n ----- \n Whoops, I'm unable to connect to the messaging queue.\n ----- \n\n{0}", ex.ToString());
                Console.ReadKey();
            }
        }

        private static void Listen(object sender, PropertyChangedEventArgs e)
        {
            var request = new Deserializer<DeliveryRequest>()
                    .UseBuffer(_receiver.ReceivedMessage)
                    .DeserializeAs(MessageType.JSON);

            Console.WriteLine(
                string.Format("{0} | Delivery scehduled for Order: {1}",
                _receiver.ReceivedHeaders["TransactionId"],
                request.OrderId));
        }

        public class DeliveryRequest
        {
            public int OrderId { get; set; }
        }
    }
}