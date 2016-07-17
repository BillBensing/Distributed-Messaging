using MessageSerializer;
using OrderEngine.Core.Model.Request;
using RabbitMQAdapter;
using System;

namespace OrderEngine.Component.InventoryMgt.DataAccess
{
    public class InventoryMgtDataAccess : IInventoryMgtDataAccess
    {
        private ISender _sender;

        public InventoryMgtDataAccess(ISender _sender)
        {
            this._sender = _sender;
        }

        public void Remove(InventoryRequest request, Guid TransactionId)
        {
            try
            {
                string id = TransactionId.ToString();
                byte[] buffer = new Serializer().UseObject(request).SerializeAs(MessageType.JSON);
                _sender.AddHeader("TransactionId", id);
                _sender.Send(buffer);
                Console.WriteLine(string.Format("{0} |    Inventory Manager notified ", id));
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Failed to update inventory. \n {0}", ex.Message.ToString()));
            }
        }
    }
}