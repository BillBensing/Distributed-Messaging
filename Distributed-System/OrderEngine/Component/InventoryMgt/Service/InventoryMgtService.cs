using OrderEngine.Component.InventoryMgt.DataAccess;
using OrderEngine.Component.InventoryMgt.Service.Strategy;
using OrderEngine.Core.Model.Request;
using System;

namespace OrderEngine.Component.InventoryMgt.Service
{
    public class InventoryMgtService : IInventoryMgtService
    {
        private IInventoryMgtDataAccess _inventoryMgtDataAccess;

        public InventoryMgtService(IInventoryMgtDataAccess InventoryMgtDataAccess)
        {
            this._inventoryMgtDataAccess = InventoryMgtDataAccess;
        }

        public void NewOrder(OrderRequest request, Guid TransactionID)
        {
            try
            {
                //Marshal Order request into Inventory Request
                InventoryRequest inventoryRequest = new InventoryRequest() { OrderId = request.Id };

                IInventoryMgtStrategy strategy = new StandardInventoryMgtStrategy(this._inventoryMgtDataAccess, inventoryRequest, TransactionID);
                strategy.Execute();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Failed to update inventory. \n {0}", ex.Message.ToString()));
            }
        }
    }
}