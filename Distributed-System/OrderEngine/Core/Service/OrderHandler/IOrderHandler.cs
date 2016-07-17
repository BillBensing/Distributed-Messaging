using OrderEngine.Core.Model;
using OrderEngine.Core.Model.Request;

namespace OrderEngine.Core.Service.OrderHandler
{
    public interface IOrderHandler
    {
        OrderState Process(OrderRequest request);

        void RegisterNext(IOrderHandler NextStep);
    }
}