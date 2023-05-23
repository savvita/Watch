using System.ComponentModel.DataAnnotations;

namespace Watch.DataAccess.UI.Models
{
    public class Watch
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        public string Model { get; set; } = null!;
        public double Rate { get; set; } = 0;
        public int Votes { get; set; } = 0;

        public Brand? Brand { get; set; }
        public Collection? Collection { get; set; }
        public Style? Style { get; set; }
        public MovementType? MovementType { get; set; }
        public GlassType? GlassType { get; set; }
        public CaseShape? CaseShape { get; set; }
        public Material? CaseMaterial { get; set; }
        public double? CaseSize { get; set; }
        public StrapType? StrapType { get; set; }
        public Color? CaseColor { get; set; }
        public Color? StrapColor { get; set; }
        public Color? DialColor { get; set; }
        public WaterResistance? WaterResistance { get; set; }
        public IncrustationType? IncrustationType { get; set; }
        public DialType? DialType { get; set; }
        public Gender? Gender { get; set; }
        public double? Weight { get; set; }
        public decimal Price { get; set; }
        public decimal? Discount { get; set; }
        public bool OnSale { get; set; }
        public bool IsTop { get; set; }
        public int Available { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }
        public List<Image> Images { get; } = new List<Image>();
        public List<Function> Functions { get; } = new List<Function>();
        public List<Review> Reviews { get; } = new List<Review>();

        [Timestamp]
        public byte[]? RowVersion { get; set; }
        public Watch()
        {
        }

        public Watch(WatchModel model)
        {
            Id = model.Id;
            Title = model.Title;
            Model = model.Model;
            CaseSize = model.CaseSize;
            Weight = model.Weight;
            Price = model.Price;
            Discount = model.Discount;
            OnSale = model.OnSale;
            IsTop = model.IsTop;
            Available = model.Available;
            Description = model.Description;
            Rate = model.Rate;
            Votes = model.Votes;
            
            if(model.Brand != null)
            {
                Brand = new Brand(model.Brand);
            }

            if (model.Collection != null)
            {
                Collection = new Collection(model.Collection);
            }

            if (model.Style != null)
            {
                Style = new Style(model.Style);
            }

            if (model.MovementType != null)
            {
                MovementType = new MovementType(model.MovementType);
            }

            if (model.GlassType != null)
            {
                GlassType = new GlassType(model.GlassType);
            }

            if (model.CaseShape != null)
            {
                CaseShape = new CaseShape(model.CaseShape);
            }

            if (model.CaseMaterial != null)
            {
                CaseMaterial = new Material(model.CaseMaterial);
            }

            if (model.StrapType != null)
            {
                StrapType = new StrapType(model.StrapType);
            }

            if (model.CaseColor != null)
            {
                CaseColor = new Color(model.CaseColor);
            }

            if (model.StrapColor != null)
            {
                StrapColor = new Color(model.StrapColor);
            }

            if (model.DialColor != null)
            {
                DialColor = new Color(model.DialColor);
            }

            if (model.WaterResistance != null)
            {
                WaterResistance = new WaterResistance(model.WaterResistance);
            }

            if (model.IncrustationType != null)
            {
                IncrustationType = new IncrustationType(model.IncrustationType);
            }

            if (model.DialType != null)
            {
                DialType = new DialType(model.DialType);
            }

            if (model.Gender != null)
            {
                Gender = new Gender(model.Gender);
            }

            model.Functions.ToList().ForEach(f => Functions.Add(new Function(f)));
            model.Reviews.ToList().ForEach(f => Reviews.Add(new Review(f)));
            model.Images.ToList().ForEach(i => Images.Add(new Image(i)));

            RowVersion = model.RowVersion;
    }

        public static explicit operator WatchModel(Watch entity)
        {        
            var model = new WatchModel()
            {
                Id = entity.Id,
                Title = entity.Title,
                Model = entity.Model,
                CaseSize = entity.CaseSize,
                Weight = entity.Weight,
                Price = entity.Price,
                Discount = entity.Discount,
                OnSale = entity.OnSale,
                IsTop = entity.IsTop,
                Available = entity.Available,
                Description = entity.Description,
                BrandId = entity.Brand?.Id,
                CollectionId = entity.Collection?.Id,
                StyleId = entity.Style?.Id,
                MovementTypeId = entity.MovementType?.Id,
                GlassTypeId = entity.GlassType?.Id,
                CaseShapeId = entity.CaseShape?.Id,
                CaseMaterialId = entity.CaseMaterial?.Id,
                StrapTypeId = entity.StrapType?.Id,
                CaseColorId = entity.CaseColor?.Id,
                StrapColorId = entity.StrapColor?.Id,
                DialColorId = entity.DialColor?.Id,
                WaterResistanceId = entity.WaterResistance?.Id,
                IncrustationTypeId = entity.IncrustationType?.Id,
                DialTypeId = entity.DialType?.Id,
                GenderId = entity.Gender?.Id,
                RowVersion = entity.RowVersion,
                Rate = entity.Rate,
                Votes = entity.Votes

            };

            entity.Functions.ForEach(f => model.Functions.Add((FunctionModel)f));
            entity.Images.ForEach(i => model.Images.Add((ImageModel)i));
            entity.Reviews.ForEach(i => model.Reviews.Add((ReviewModel)i));


            return model;
        }
    }
}
