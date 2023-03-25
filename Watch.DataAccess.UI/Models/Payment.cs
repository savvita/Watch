using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watch.DataAccess.UI.Models
{
    public class Payment
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Value { get; set; } = null!;
        public bool IsActive { get; set; }

        public Payment()
        {
        }

        public Payment(PaymentModel model)
        {
            Id = model.Id;
            Value = model.Value;
            IsActive = model.IsActive;
        }

        public static explicit operator PaymentModel(Payment entity)
        {
            return new PaymentModel()
            {
                Id = entity.Id,
                Value = entity.Value,
                IsActive = entity.IsActive,
            };
        }
    }
}
