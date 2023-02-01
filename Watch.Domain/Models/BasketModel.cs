namespace Watch.Domain.Models
{
    public class BasketModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; } = null!;
        public virtual ICollection<BasketDetailModel> Details { get; } = new List<BasketDetailModel>();
    }
}
