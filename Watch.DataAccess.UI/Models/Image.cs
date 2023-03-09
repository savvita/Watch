using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watch.DataAccess.UI.Models
{
    public class Image
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(300)]
        public string Value { get; set; } = null!;

        public Image()
        {
        }

        public Image(ImageModel model)
        {
            Id = model.Id;
            Value = model.Value;
        }

        public static explicit operator ImageModel(Image entity)
        {
            return new ImageModel()
            {
                Id = entity.Id,
                Value = entity.Value
            };
        }
    }
}
