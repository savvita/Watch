using System.ComponentModel.DataAnnotations;
using Watch.NovaPoshta.Models;

namespace Watch.DataAccess.UI.Models
{
    public class Warehouse
    {
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

        public Warehouse()
        {
        }

        public Warehouse(Domain.Models.WarehouseModel model)
        {
            Ref = model.Ref;
            Number = model.Number;
            Description = model.Description;
            WarehouseStatus = model.WarehouseStatus;
            CityRef = model.CityRef;
            SettlementDescription = model.SettlementDescription;
            SettlementAreaDescription = model.SettlementAreaDescription;
        }

        public static explicit operator Domain.Models.WarehouseModel(Warehouse entity)
        {
            return new Domain.Models.WarehouseModel()
            {
                Ref = entity.Ref,
                Number = entity.Number,
                Description = entity.Description,
                WarehouseStatus = entity.WarehouseStatus,
                CityRef = entity.CityRef,
                SettlementDescription = entity.SettlementDescription,
                SettlementAreaDescription = entity.SettlementAreaDescription
            };
        }
    }
}
