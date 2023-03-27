namespace Watch.NovaPoshta.Models
{
    public class ResponseBodyContent<T> where T : class
    {
        public bool success { get; set; }
        public List<T>? data { get; set; }
    }
}
