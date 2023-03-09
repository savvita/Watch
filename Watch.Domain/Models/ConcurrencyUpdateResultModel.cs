namespace Watch.Domain.Models
{
    public class ConcurrencyUpdateResultModel
    {
        public int Code { get; set; }
        public string Message { get; set; } = String.Empty;
    }
}
