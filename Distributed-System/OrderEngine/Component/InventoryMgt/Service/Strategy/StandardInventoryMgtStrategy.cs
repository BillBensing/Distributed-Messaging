using OrderEngine.Component.InventoryMgt.DataAccess;
using OrderEngine.Core.Model.Request;
using System;

namespace OrderEngine.Component.InventoryMgt.Service.Strategy
{
    public class StandardInventoryMgtStrategy : IInventoryMgtStrategy
    {
        private IInventoryMgtDataAccess _dataAccess;
        private InventoryRequest _request;
        private Guid _transId;

        public StandardInventoryMgtStrategy(IInventoryMgtDataAccess DataAccess, InventoryRequest Request, Guid TransactionId)
        {
            ValidateConstructionArguments(DataAccess, Request, TransactionId);
            this._dataAccess = DataAccess;
            this._request = Request;
            this._transId = TransactionId;
        }

        public void Execute()
        {
            try
            {
                _dataAccess.Remove(this._request, this._transId);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Failed to update inventory. \n {0}", ex.Message.ToString()));
            }
        }

        private void ValidateConstructionArguments(IInventoryMgtDataAccess DataAccess, InventoryRequest Request, Guid TransactionId)
        {
            if (DataAccess == null || Request == null || TransactionId == null)
            {
                throw new ArgumentNullException(string.Format("Cannot construct a {0}.  You have supplied a null IInventoryMgtDataAccess, InventoryRequest or TranssactionID Guid.", this.GetType().ToString()));
            }
        }
    }
}