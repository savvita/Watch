

namespace Watch.DataAccess.UI.Models
{
    public class Sale
    {
        public DateTime Date { get; set; }
        public Watch Watch { get; set; } = null!;
        public int Count { get; set; }
        public decimal UnitPrice { get; set; }

        public Sale(SaleModel model)
        {
            Date = model.Date;
            Watch = new Watch(model.Watch);
            Count = model.Count;
            UnitPrice = model.UnitPrice;
        }
    }
}
