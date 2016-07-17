using MessageSerializer;
using OrderEngine.Component.DeliveryMgt.DataAccess;
using OrderEngine.Component.DeliveryMgt.Service;
using OrderEngine.Component.InventoryMgt.DataAccess;
using OrderEngine.Component.InventoryMgt.Service;
using OrderEngine.Component.OrderEngine.Service;
using OrderEngine.Component.OrderMgt.Service;
using OrderEngine.Component.Payment.Service;
using OrderEngine.Core.Model.Request;
using RabbitMQ.Client;
using RabbitMQAdapter;
using RabbitMQAdapter.Configuration;
using System;
using System.ComponentModel;

namespace OrderEngine
{
    internal class Program
    {
        private static IOrderEngineService _engineSvc;
        private static IModel _receiverModel, _inventoryMgtModel, _deliveryMgtModel;
        private static IConnection _connection;
        private static Listener _clientReceiver;
        private static ISender _inventoryMgtSender, _deliveryMgtSender;

        private static void Main(string[] args)
        {
            try
            {
                // Set up Connection to queue
                _connection = new ConnectionAdapter(DemoConnection.HOST, DemoConnection.USER, DemoConnection.PW).GetConnection();

                // Instantiate Channel senders
                _inventoryMgtModel = _connection.CreateModel();
                _inventoryMgtSender = new Sender(_inventoryMgtModel, "InventoryMgt.FillOrder", "");

                _deliveryMgtModel = _connection.CreateModel();
                _deliveryMgtSender = new Sender(_deliveryMgtModel, "DeliveryMgt.NewDelivery", "");

                // Instantiate Channel listeners
                _receiverModel = _connection.CreateModel();
                _clientReceiver = new Listener(_receiverModel, "OrderEngine", "");

                // Inject dependencies
                _engineSvc = new OrderEngineService(
                        new InventoryMgtService(
                            new InventoryMgtDataAccess(_inventoryMgtSender))
                    ,   new DeliveryMgtService(
                            new DeliveryMgtDataAccess(_deliveryMgtSender)));

                // Start Listening for messages
                _clientReceiver.PropertyChanged += Listen;
                _clientReceiver.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\n ----- \n Whoops, I'm unable to connect to the messaging queue.\n ----- \n\n{0}", ex.ToString());
                Console.ReadKey();
            }
        }

        private static void Listen(object sender, PropertyChangedEventArgs e)
        {
            Guid transactionId = Guid.NewGuid();
            OrderRequest request;
            try
            {
                // Deserialize requests
                request = new Deserializer<OrderRequest>().UseBuffer(_clientReceiver.ReceivedMessage).DeserializeAs(MessageType.JSON);

                // Process Orders
                _engineSvc.ProcessOrder(request);
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0} | ERROR: Orchestration Order failed: \n {1}", transactionId.ToString(), ex.Message);
            }
        }
    }
}