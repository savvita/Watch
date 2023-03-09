namespace Watch.Domain.Models
{
    public class ResultModel<T>
    {
        public T? Value { get; set; } = default(T);
        public int Hits { get; set; }
    }
}
