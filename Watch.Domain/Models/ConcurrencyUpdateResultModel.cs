namespace Watch.Domain.Models
{
    public class ConcurrencyUpdateResultModel<T> where T : class
    {
        public int Code { get; set; }
        public string Message { get; set; } = String.Empty;
        public T? ConcurrencyConflictedObject { get; set; }
    }
}
