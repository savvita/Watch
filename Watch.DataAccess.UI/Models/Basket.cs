namespace Watch.DataAccess.UI.Models
{
    public class Basket
    {
        public int Id { get; set; }

        public string UserId { get; set; } = null!;

        public List<BasketDetail> Details { get; set; } = new List<BasketDetail>();

        public Basket()
        {
        }

        public Basket(BasketModel model)
        {
            Id = model.Id;
            UserId = model.UserId;
            model.Details.ToList().ForEach(d => Details.Add(new BasketDetail(d)));
        }

        public static explicit operator BasketModel(Basket entity)
        {
            var model = new BasketModel()
            {
                Id = entity.Id,
                UserId = entity.UserId
            };

            entity.Details.ForEach(detail => model.Details.Add((BasketDetailModel)detail));
            return model;
        }
    }
}
