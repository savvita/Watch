using System.ComponentModel.DataAnnotations;
using Watch.Domain.Models;

namespace Watch.DataAccess.UI.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; } = null!;

        public Category()
        {
        }

        public Category(CategoryModel model)
        {
            Id = model.Id;
            CategoryName = model.CategoryName;
        }

        public static explicit operator CategoryModel(Category category)
        {
            return new CategoryModel()
            {
                Id = category.Id,
                CategoryName = category.CategoryName
            };
        }
    }
}
