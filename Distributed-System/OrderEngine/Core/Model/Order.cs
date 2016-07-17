namespace OrderEngine.Core.Model
{
    public class Order
    {
        public int Id { get; set; }
        public OrderState State { get; set; }
    }
}