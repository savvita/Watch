using System.ComponentModel.DataAnnotations;
using Watch.Domain.Models;

namespace Watch.DataAccess.UI.Models
{
    public class Producer
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string ProducerName { get; set; } = null!;

        public Producer()
        {
        }

        public Producer(ProducerModel model)
        {
            Id = model.Id;
            ProducerName = model.ProducerName;
        }

        public static explicit operator ProducerModel(Producer producer)
        {
            return new ProducerModel()
            {
                Id = producer.Id,
                ProducerName = producer.ProducerName
            };
        }
    }
}
