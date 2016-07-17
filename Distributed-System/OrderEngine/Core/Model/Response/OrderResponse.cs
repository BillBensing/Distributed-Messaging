namespace OrderEngine.Core.Model.Response
{
    public class OrderResponse
    {
        public bool PaymentApproved { get; set; }
        public int OrderNumber { get; set; }
        public OrderState OrderState { get; set; }
    }
}