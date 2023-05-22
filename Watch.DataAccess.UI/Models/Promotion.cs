using System.ComponentModel.DataAnnotations;

namespace Watch.DataAccess.UI.Models
{
    public class Promotion
    {
        public int Id { get; set; }
        [Timestamp]
        public byte[]? RowVersion { get; set; }
        [MaxLength(100)]
        public string Title { get; set; } = null!;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal DiscountValue { get; set; } = 0;

        [MaxLength(1000)]
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public Brand? Brand { get; set; }

        public Promotion()
        {
        }

        public Promotion(PromotionModel model)
        {
            Id = model.Id;
            Title = model.Title;
            StartDate = model.StartDate;
            EndDate = model.EndDate;
            DiscountValue = model.DiscountValue;
            Description = model.Description;
            IsActive = model.IsActive;
            RowVersion = model.RowVersion;

            if (model.Brand != null)
            {
                Brand = new Brand(model.Brand);
            }
        }

        public static explicit operator PromotionModel(Promotion entity)
        {
            return new PromotionModel()
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                IsActive = entity.IsActive,
                DiscountValue = entity.DiscountValue,
                BrandId = entity.Brand != null ? entity.Brand.Id : null,
                RowVersion = entity.RowVersion
            };
        }
    }
}
