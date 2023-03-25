using System.ComponentModel.DataAnnotations;

namespace Watch.NovaPoshta.Models
{
    public class WarehouseModel
    {
        [Required]
        [MaxLength(36)]
        public string Ref { get; set; } = null!;
  
        [Required]
        [MaxLength(99)]
        public string Description { get; set; } = null!;
        [Required]
        [MaxLength(36)]
        public string WarehouseStatus { get;set; } = null!;

        [MaxLength(36)]
        public string? Number { get; set; }

        [Required]
        [MaxLength(36)]
        public string CityRef { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string SettlementDescription { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string SettlementAreaDescription { get; set; } = null!;

    }
}
