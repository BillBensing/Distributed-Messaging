using OrderEngine.Core.Model;
using OrderEngine.Core.Model.Request;
using System;

namespace OrderEngine.Core.Service.OrderHandler
{
    /// <summary>
    /// Class which provides the last link in the Chain of Responsiblity
    /// for the OrderHandler chain.  If the chain getst his far, then it is a completly
    /// valid OrderRequest.
    /// </summary>
    public class OrderHandlerSuccess : IOrderHandler
    {
        private static OrderHandlerSuccess _instance;

        private OrderHandlerSuccess()
        {
        }

        public static OrderHandlerSuccess GetInstance()
        {
            if (_instance == null)
            {
                _instance = new OrderHandlerSuccess();
            }
            return _instance;
        }

        public OrderState Process(OrderRequest request)
        {
            return OrderState.Valid;
        }

        public void RegisterNext(IOrderHandler NextStep)
        {
            throw new Exception("Cannot register another link in the chain, this is the last link");
        }
    }
}