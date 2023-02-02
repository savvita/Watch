using Watch.Domain.Models;

namespace Watch.DataAccess.UI.Models
{
    public class Basket
    {
        public int Id { get; set; }

        public string UserId { get; set; } = null!;

        public DateTime Date { get; set; }

        public List<BasketDetail> Details { get; set; } = new List<BasketDetail>();

        public Basket()
        {
        }

        public Basket(BasketModel model)
        {
            Id = model.Id;
            Date = model.Date;
            UserId = model.UserId;
        }

        public static explicit operator BasketModel(Basket basket)
        {
            var model = new BasketModel()
            {
                Id = basket.Id,
                Date = basket.Date,
                UserId = basket.UserId
            };

            basket.Details.ForEach(detail => model.Details.Add((BasketDetailModel)detail));
            return model;
        }
    }
}
