using System.ComponentModel.DataAnnotations;
using Watch.Domain.Models;

namespace Watch.DataAccess.UI.Models
{
    public class Category
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string? CategoryName { get; set; }

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
            if (category.CategoryName == null)
            {
                throw new ArgumentNullException();
            }

            return new CategoryModel()
            {
                Id = category.Id,
                CategoryName = category.CategoryName
            };
        }
    }
}
