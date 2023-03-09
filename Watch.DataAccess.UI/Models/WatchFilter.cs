namespace Watch.DataAccess.UI.Models
{
    public class WatchFilter
    {
        public string Model { get; set; } = String.Empty;
        public List<int?> BrandId { get; set; } = new List<int?>();
        public List<int?> CollectionId { get; set; } = new List<int?>();
        public List<int?> StyleId { get; set; } = new List<int?>();
        public List<int?> MovementTypeId { get; set; } = new List<int?>();
        public List<int?> GlassTypeId { get; set; } = new List<int?>();
        public List<int?> CaseShapeId { get; set; } = new List<int?>();
        public List<int?> CaseMaterialId { get; set; } = new List<int?>();
        public List<int?> StrapTypeId { get; set; } = new List<int?>();
        public List<int?> CaseColorId { get; set; } = new List<int?>();
        public List<int?> StrapColorId { get; set; } = new List<int?>();
        public List<int?> DialColorId { get; set; } = new List<int?>();
        public List<int?> WaterResistanceId { get; set; } = new List<int?>();
        public List<int?> IncrustationTypeId { get; set; } = new List<int?>();
        public List<int?> DialTypeId { get; set; } = new List<int?>();
        public List<int?> GenderId { get; set; } = new List<int?>();
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public List<bool> OnSale { get; set; } = new List<bool>();
        public List<bool> IsTop { get; set; } = new List<bool>();

        public static explicit operator WatchFilterModel(WatchFilter entity)
        {
            return new WatchFilterModel()
            {
                Model = entity.Model,
                BrandId = entity.BrandId,
                CollectionId = entity.CollectionId,
                StyleId = entity.StyleId,
                MovementTypeId = entity.MovementTypeId,
                GlassTypeId = entity.GlassTypeId,
                CaseShapeId = entity.CaseShapeId,
                CaseMaterialId = entity.CaseMaterialId,
                StrapTypeId = entity.StrapTypeId,
                CaseColorId = entity.CaseColorId,
                StrapColorId = entity.StrapColorId,
                DialColorId = entity.DialColorId,
                WaterResistanceId = entity.WaterResistanceId,
                IncrustationTypeId = entity.IncrustationTypeId,
                DialTypeId = entity.DialTypeId,
                GenderId = entity.GenderId,
                MinPrice = entity.MinPrice,
                MaxPrice = entity.MaxPrice,
                OnSale = entity.OnSale,
                IsTop = entity.IsTop
            };
        }
    }
}
