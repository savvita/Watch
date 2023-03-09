namespace Watch.Domain.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; } = null!;
        public int StatusId { get; set; }
        public string? ManagerId { get; set; }
        public virtual OrderStatusModel? Status { get; set; }
        public virtual ICollection<OrderDetailModel> Details { get; } = new List<OrderDetailModel>();
        public virtual UserModel? Manager { get; set; }
    }
}
