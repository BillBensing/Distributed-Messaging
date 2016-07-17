using OrderEngine.Core.Model.Request;
using System;

namespace OrderEngine.Component.DeliveryMgt.Service
{
    public interface IDeliveryMgtService
    {
        void NewDelivery(OrderRequest request, Guid TransactionId);
    }
}