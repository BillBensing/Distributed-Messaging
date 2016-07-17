using OrderEngine.Core.Model;
using OrderEngine.Core.Model.Request;
using OrderEngine.Core.Service.OrderHandler;
using System;

namespace OrderEngine.Component.OrderMgt.Service.OrderHandler
{
    public class OrderMgtApprover : IOrderApprover
    {
        public OrderState ProcessOrder(OrderRequest request)
        {
            Random rand = new Random();
            int id = rand.Next(0, 1000);
            request.Id = id;

            Console.WriteLine(string.Format("Order Number {0} has been created", request.Id.ToString()));
            return OrderState.Valid;
        }
    }
}