using OrderEngine.Core.Model.Request;
using System;

namespace OrderEngine.Component.InventoryMgt.Service
{
    public interface IInventoryMgtService
    {
        void NewOrder(OrderRequest request, Guid TransactionID);
    }
}