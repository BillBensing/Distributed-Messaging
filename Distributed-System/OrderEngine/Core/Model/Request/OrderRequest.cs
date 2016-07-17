namespace OrderEngine.Core.Model.Request
{
    public class OrderRequest
    {
        public string Customer { get; set; }
        public string Product { get; set; }
        public string Payment { get; set; }
        public int Id { get; set; }
    }
}