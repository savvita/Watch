using System.ComponentModel.DataAnnotations;

namespace Watch.DataAccess.UI.Models
{
    public class Slide
    {
        public int Id { get; set; }
        [Timestamp]
        public byte[]? RowVersion { get; set; }

        [MaxLength(255)]
        public string? ImageUrl { get; set; }

        public int Index { get; set; }

        public bool IsActive { get; set; }
        public virtual List<SlideText> Texts { get; } = new List<SlideText>();
        public virtual Promotion? Promotion { get; set; }

        public Slide()  
        {
        }

        public Slide(SlideModel model)
        {
            Id = model.Id;
            ImageUrl = model.ImageUrl;
            Index = model.Index;
            IsActive = model.IsActive;
            Promotion = model.Promotion != null ? new Promotion(model.Promotion) : null;
            foreach(var text in model.Texts)
            {
                Texts.Add(new SlideText(text));
            }
            RowVersion = model.RowVersion;
        }

        public static explicit operator SlideModel(Slide entity)
        {
            var model = new SlideModel()
            {
                Id = entity.Id,
                ImageUrl = entity.ImageUrl,
                Index = entity.Index,
                IsActive = entity.IsActive,
                PromotionId = entity.Promotion != null ? entity.Promotion.Id : null,
                RowVersion = entity.RowVersion
            };

            foreach(var text in entity.Texts)
            {
                model.Texts.Add((TextModel)text);
            }

            return model;
        }
    }
}
