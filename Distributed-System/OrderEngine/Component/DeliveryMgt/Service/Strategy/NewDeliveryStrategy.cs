using OrderEngine.Component.DeliveryMgt.DataAccess;
using OrderEngine.Core.Model.Request;
using System;

namespace OrderEngine.Component.DeliveryMgt.Service.Strategy
{
    public class NewDeliveryStrategy : IDeliveryMgtStrategy
    {
        private IDeliveryMgtDataAccess _dataAccess;
        private DeliveryRequest _request;
        private Guid _transId;

        public NewDeliveryStrategy(IDeliveryMgtDataAccess DeliveryMgtDataAccess, DeliveryRequest Request, Guid TransactionId)
        {
            this._dataAccess = DeliveryMgtDataAccess;
            this._request = Request;
            this._transId = TransactionId;
        }

        public void Execute()
        {
            try
            {
                _dataAccess.Create(this._request, this._transId);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Failed to update inventory. \n {0}", ex.Message.ToString()));
            }
        }

        private void ValidateConstructionArguments(IDeliveryMgtDataAccess DataAccess, InventoryRequest Request, Guid TransactionId)
        {
            if (DataAccess == null || Request == null || TransactionId == null)
            {
                throw new ArgumentNullException("Cannot construct a {0}.  You have supplied a null IDeliveryMgtDataAccess, InventoryRequest or TranssactionID Guid.", this.GetType().ToString());
            }
        }
    }
}