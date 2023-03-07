using System.ComponentModel.DataAnnotations;

namespace Watch.DataAccess.UI.Models
{
    public class WaterResistance
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Value { get; set; } = null!;

        public WaterResistance()
        {
        }

        public WaterResistance(WaterResistanceModel model)
        {
            Id = model.Id;
            Value = model.Value;
        }

        public static explicit operator WaterResistanceModel(WaterResistance entity)
        {
            return new WaterResistanceModel()
            {
                Id = entity.Id,
                Value = entity.Value
            };
        }
    }
}
