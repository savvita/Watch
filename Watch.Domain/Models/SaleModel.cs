namespace Watch.Domain.Models
{
    public class SaleModel
    {
        public DateTime Date { get; set; }
        public WatchModel Watch { get; set; } = null!;
        public int Count { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
