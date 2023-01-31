using System.ComponentModel.DataAnnotations;
using Watch.Domain.Models;

namespace Watch.DataAccess.UI.Models
{
    public class Producer
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string? ProducerName { get; set; }

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
            if(producer.ProducerName == null)
            {
                throw new ArgumentNullException();
            }
            return new ProducerModel()
            {
                Id = producer.Id,
                ProducerName = producer.ProducerName
            };
        }
    }
}
