using OrderEngine.Core.Model.Request;
using System;

namespace OrderEngine.Component.DeliveryMgt.DataAccess
{
    public interface IDeliveryMgtDataAccess
    {
        void Create(DeliveryRequest reqeust, Guid TransactionId);
    }
}