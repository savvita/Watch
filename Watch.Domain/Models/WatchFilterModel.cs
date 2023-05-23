namespace Watch.Domain.Models
{
    public class WatchFilterModel
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
        public List<int?> FunctionId { get; set; } = new List<int?>();
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public List<bool> OnSale { get; set; } = new List<bool>();
        public List<bool> IsTop { get; set; } = new List<bool>();
        public string? Sorting { get; set; }
        public string? SortingOrder { get; set; }
    }
}
