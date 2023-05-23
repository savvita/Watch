using System.ComponentModel.DataAnnotations;

namespace Watch.Domain.Models
{
    public class WatchModel
    {
        public int Id { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        public string Model { get; set; } = null!;

        public double Rate { get; set; } = 0;
        public int Votes { get; set; } = 0;

        public int? BrandId { get; set; }
        public int? CollectionId{ get; set; }
        public int? StyleId { get; set; }
        public int? MovementTypeId { get; set; }
        public int? GlassTypeId { get; set; }
        public int? CaseShapeId { get; set; }
        public int? CaseMaterialId { get; set; }
        public double? CaseSize { get; set; }
        public int? StrapTypeId { get; set; }
        public int? CaseColorId { get; set; }
        public int? StrapColorId { get; set; }
        public int? DialColorId { get; set; }
        public int? WaterResistanceId { get; set; }
        public int? IncrustationTypeId { get; set; }
        public int? DialTypeId { get; set; }
        public int? GenderId { get; set; }
        public double? Weight { get; set; }
        public decimal Price { get; set; }

        [Range(0, 100)]
        public decimal? Discount { get; set; }
        public bool OnSale { get; set; }
        public bool IsTop { get; set; }
        public int Available { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        public virtual BrandModel? Brand { get; set; }
        public virtual CollectionModel? Collection { get; set; }
        public virtual StyleModel? Style { get; set; }
        public virtual MovementTypeModel? MovementType { get; set; }
        public virtual GlassTypeModel? GlassType { get; set; }
        public virtual CaseShapeModel? CaseShape { get; set; }
        public virtual MaterialModel? CaseMaterial { get; set; }
        public virtual StrapTypeModel? StrapType { get; set; }
        public virtual ColorModel? CaseColor { get; set; }
        public virtual ColorModel? StrapColor { get; set; }
        public virtual ColorModel? DialColor { get; set; }
        public virtual WaterResistanceModel? WaterResistance { get; set; }
        public virtual IncrustationTypeModel? IncrustationType { get; set; }
        public virtual DialTypeModel? DialType { get; set; }
        public virtual GenderModel? Gender { get; set; }

        public virtual ICollection<ImageModel> Images { get; } = new List<ImageModel>();
        public virtual ICollection<FunctionModel> Functions { get; } = new List<FunctionModel>();
        public virtual ICollection<ReviewModel> Reviews { get; } = new List<ReviewModel>();
    }
}
