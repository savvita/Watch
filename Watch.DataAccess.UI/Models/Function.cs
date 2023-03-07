using System.ComponentModel.DataAnnotations;

namespace Watch.DataAccess.UI.Models
{
    public class Function
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Value { get; set; } = null!;

        public Function()
        {
        }

        public Function(FunctionModel model)
        {
            Id = model.Id;
            Value = model.Value;
        }

        public static explicit operator FunctionModel(Function entity)
        {
            return new FunctionModel()
            {
                Id = entity.Id,
                Value = entity.Value
            };
        }
    }
}
