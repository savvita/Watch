namespace Watch.Domain.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }
        public int StatusId { get; set; }
        public virtual OrderStatusModel? Status { get; set; }
        public virtual ICollection<OrderDetailModel> Details { get; } = new List<OrderDetailModel>();
    }
}
