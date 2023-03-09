using System.ComponentModel.DataAnnotations;

namespace Watch.DataAccess.UI.Models
{
    public class Collection
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Value { get; set; } = null!;

        public Collection()
        {
        }

        public Collection(CollectionModel model)
        {
            Id = model.Id;
            Value = model.Value;
        }

        public static explicit operator CollectionModel(Collection entity)
        {
            return new CollectionModel()
            {
                Id = entity.Id,
                Value = entity.Value
            };
        }
    }
}
