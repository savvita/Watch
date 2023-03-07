using System.ComponentModel.DataAnnotations;

namespace Watch.DataAccess.UI.Models
{
    public class CaseShape
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Value { get; set; } = null!;

        public CaseShape()
        {
        }

        public CaseShape(CaseShapeModel model)
        {
            Id = model.Id;
            Value = model.Value;
        }

        public static explicit operator CaseShapeModel(CaseShape entity)
        {
            return new CaseShapeModel()
            {
                Id = entity.Id,
                Value = entity.Value
            };
        }
    }
}
