using System.ComponentModel.DataAnnotations;
using Watch.Domain.Models;

namespace Watch.DataAccess.UI.Models
{
    public class Watch
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Model { get; set; } = null!;

        [Required]
        public decimal Price { get; set; }

        public Category? Category { get; set; }

        public Producer? Producer { get; set; }

        public int Available { get; set; }

        public bool OnSale { get; set; }
        public int Sold { get; set; }
        public bool IsPopular { get; set; }

        public string? ImageUrl { get; set; }

        public Watch()
        {
        }

        public Watch(WatchModel model)
        {
            Id = model.Id;
            Model = model.Model;
            Price = model.Price;
            Available = model.Available;
            OnSale = model.OnSale;
            IsPopular = model.IsPopular;
            Sold = model.Sold;
            ImageUrl = model.ImageUrl;

            if (model.Category != null)
            {
                Category = new Category(model.Category);
            }

            if (model.Producer != null)
            {
                Producer = new Producer(model.Producer);
            }
        }

        public static explicit operator WatchModel(Watch watch)
        {
            return new WatchModel()
            {
                Id = watch.Id,
                Model = watch.Model,
                Available = watch.Available,
                OnSale = watch.OnSale,
                IsPopular = watch.IsPopular,
                Sold = watch.Sold,
                ImageUrl = watch.ImageUrl,
                Price = watch.Price,
                CategoryId = watch.Category != null ? watch.Category.Id : 0,
                ProducerId = watch.Producer != null ? watch.Producer.Id : 0
            };
        }
    }
}
