using MessageSerializer;
using OrderEngine.Core.Model.Request;
using RabbitMQAdapter;
using System;

namespace OrderEngine.Component.DeliveryMgt.DataAccess
{
    public class DeliveryMgtDataAccess : IDeliveryMgtDataAccess
    {
        private ISender _sender;

        public DeliveryMgtDataAccess(ISender sender)
        {
            this._sender = sender;
        }

        public void Create(DeliveryRequest request, Guid TransactionId)
        {
            try
            {
                string id = TransactionId.ToString();
                byte[] buffer = new Serializer().UseObject(request).SerializeAs(MessageType.JSON);
                _sender.AddHeader("TransactionId", id);
                _sender.Send(buffer);
                Console.WriteLine(string.Format("{0} |    Delivery Manager notified ", id));
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Failed to update inventory. \n {0}", ex.Message.ToString()));
            }
        }
    }
}