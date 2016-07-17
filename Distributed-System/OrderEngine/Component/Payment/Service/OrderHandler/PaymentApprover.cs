using OrderEngine.Core.Model;
using OrderEngine.Core.Model.Request;
using OrderEngine.Core.Service.OrderHandler;
using System;

namespace OrderEngine.Component.Payment.Service.OrderHandler
{
    public class PaymentApprover : IOrderApprover
    {
        public OrderState ProcessOrder(OrderRequest request)
        {
            //Console.WriteLine("Payment Declined");
            //return OrderState.Invalid;
            Console.WriteLine(string.Format("Payment processed for {0}", request.Payment));
            return OrderState.Valid;
        }
    }
}