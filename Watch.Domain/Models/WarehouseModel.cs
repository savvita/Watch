using System.ComponentModel.DataAnnotations;

namespace Watch.Domain.Models
{
    public class WarehouseModel
    {
        [Key]
        [Required]
        [MaxLength(36)]
        public string Ref { get; set; } = null!;

        [MaxLength(36)]
        public string? Number { get; set; }

        [Required]
        [MaxLength(99)]
        public string Description { get; set; } = null!;
        [Required]
        [MaxLength(36)]
        public string WarehouseStatus { get; set; } = null!;

        [Required]
        [MaxLength(36)]
        public string CityRef { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string SettlementDescription { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string SettlementAreaDescription { get; set; } = null!;

        [Required]
        public DateTime LastUpdate { get; set; }

        public WarehouseModel()
        {

        }
        public WarehouseModel(NovaPoshta.Models.WarehouseModel model)
        {
            Ref = model.Ref;
            Number = model.Number;
            Description = model.Description;
            WarehouseStatus = model.WarehouseStatus;
            CityRef = model.CityRef;
            SettlementDescription = model.SettlementDescription;
            SettlementAreaDescription = model.SettlementAreaDescription;
            LastUpdate = DateTime.Now;
        }
    }
}
