namespace Watch.Domain.Models
{
    public class BasketDetailModel
    {
        public int Id { get; set; }
        public int BasketId { get; set; }
        public int WatchId { get; set; }
        public int Count { get; set; }
        public decimal UnitPrice { get; set; }
        public virtual WatchModel? Watch { get; set; }
    }
}
