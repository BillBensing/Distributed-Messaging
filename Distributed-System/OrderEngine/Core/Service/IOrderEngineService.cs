using OrderEngine.Core.Model.Request;

namespace OrderEngine.Component.OrderEngine.Service
{
    public interface IOrderEngineService
    {
        void ProcessOrder(OrderRequest request);
    }
}