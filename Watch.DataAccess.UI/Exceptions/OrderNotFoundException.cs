namespace Watch.DataAccess.UI.Exceptions
{
    public class OrderNotFoundException : Exception
    {
        public int OrderId { get; }
        public OrderNotFoundException(int orderId) : base($"Order with Id {orderId} not found")
        {
            OrderId = orderId;
        }
    }
}
