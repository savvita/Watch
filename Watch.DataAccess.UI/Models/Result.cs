namespace Watch.DataAccess.UI.Models
{
    public class Result<T>
    {
        public string? Token { get; set; }
        public T? Value { get; set; }
        public int Hits { get; set; }

        public Result()
        {

        }

        public Result(ResultModel<T> entity)
        {
            Value = entity.Value;
            Hits = entity.Hits;
        }
    }
}
