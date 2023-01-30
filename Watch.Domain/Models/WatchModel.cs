namespace Watch.Domain.Models
{
    public class WatchModel
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int ProducerId { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public int Available { get; set; }
        public int Sold { get; set; }
        public bool OnSale { get; set; }
        public bool IsPopular { get; set; }
        public virtual CategoryModel? Category { get; set; }
        public virtual ProducerModel? Producer { get; set; }
    }
}
