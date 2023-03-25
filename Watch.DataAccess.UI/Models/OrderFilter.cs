namespace Watch.DataAccess.UI.Models
{
    public class OrderFilter
    {
        public int? WatchId { get; set; }
        public int? BrandId { get; set; }
        public int? CollectionId { get; set; }
        public int? StyleId { get; set; }
        public int? MovementTypeId { get; set; }
        public int? GlassTypeId { get; set; }
        public int? CaseShapeId { get; set; }
        public int? CaseMaterialId { get; set; }
        public int? CountryId { get; set; }
        public int? FunctionId { get; set; }
        public int? StrapTypeId { get; set; }
        public int? CaseColorId { get; set; }
        public int? StrapColorId { get; set; }
        public int? DialColorId { get; set; }
        public int? IncrustationTypeId { get; set; }
        public int? WaterResistanceId { get; set; }
        public int? DialTypeId { get; set; }
        public int? GenderId { get; set; }

        public static explicit operator OrderFilterModel(OrderFilter entity)
        {
            return new OrderFilterModel()
            {
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
                CountryId = entity.CountryId,
                FunctionId = entity.FunctionId,
                WatchId = entity.WatchId
            };
        }
    }
}
