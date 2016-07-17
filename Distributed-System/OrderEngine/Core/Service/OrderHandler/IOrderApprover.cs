using OrderEngine.Core.Model;
using OrderEngine.Core.Model.Request;

namespace OrderEngine.Core.Service.OrderHandler
{
    public interface IOrderApprover
    {
        OrderState ProcessOrder(OrderRequest request);
    }
}