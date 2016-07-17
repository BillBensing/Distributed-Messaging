using OrderEngine.Component.DeliveryMgt.DataAccess;
using OrderEngine.Component.DeliveryMgt.Service.Strategy;
using OrderEngine.Core.Model.Request;
using System;

namespace OrderEngine.Component.DeliveryMgt.Service
{
    public class DeliveryMgtService : IDeliveryMgtService
    {
        private IDeliveryMgtDataAccess _deliveryMgtDataAccess;

        public DeliveryMgtService(IDeliveryMgtDataAccess DeliveryMgtDataAccess)
        {
            this._deliveryMgtDataAccess = DeliveryMgtDataAccess;
        }

        public void NewDelivery(OrderRequest request, Guid TransactionId)
        {
            try
            {
                //Marshal Order request into Inventory Request
                DeliveryRequest deliveryRequest = new DeliveryRequest() { OrderId = request.Id };

                IDeliveryMgtStrategy strategy = new NewDeliveryStrategy(this._deliveryMgtDataAccess, deliveryRequest, TransactionId);
                strategy.Execute();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Failed to update inventory. \n {0}", ex.Message.ToString()));
            }
        }
    }
}