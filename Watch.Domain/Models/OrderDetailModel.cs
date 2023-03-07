namespace Watch.Domain.Models
{
    public class OrderDetailModel
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int WatchId { get; set; }
        public int Count { get; set; }
        public decimal UnitPrice { get; set; }
        public virtual WatchModel? Watch { get; set; }
        public virtual OrderModel? Order { get; set; }
    }
}
