namespace Watch.DataAccess.UI.Models
{
    public class ConcurrencyUpdateResult<T> where T : class
    {
        public int Code { get; set; }
        public string Message { get; set; } = String.Empty;
        public T? ConcurrencyConflictedObject { get; set; }
    }
}
