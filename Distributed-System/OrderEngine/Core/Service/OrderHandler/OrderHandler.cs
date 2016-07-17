using OrderEngine.Core.Model;
using OrderEngine.Core.Model.Request;

namespace OrderEngine.Core.Service.OrderHandler
{
    public class OrderHandler : IOrderHandler
    {
        private readonly IOrderApprover _approver;
        private IOrderHandler _next;

        public OrderHandler(IOrderApprover approver)
        {
            _approver = approver;
            _next = OrderHandlerSuccess.GetInstance();
        }

        public OrderState Process(OrderRequest request)
        {
            OrderState state = _approver.ProcessOrder(request);
            if (state == OrderState.Valid)
            {
                return _next.Process(request);
            }
            return state;
        }

        public void RegisterNext(IOrderHandler NextStep)
        {
            _next = NextStep;
        }
    }
}