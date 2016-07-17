using OrderEngine.Core.Model.Request;
using System;

namespace OrderEngine.Component.InventoryMgt.DataAccess
{
    public interface IInventoryMgtDataAccess
    {
        void Remove(InventoryRequest request, Guid TransactionId);
    }
}