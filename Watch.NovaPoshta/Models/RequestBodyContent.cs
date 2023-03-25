namespace Watch.NovaPoshta.Models
{
    [Serializable]
    public class RequestBodyContent
    {
        public string? apiKey { get; set; }
        public string? modelName { get; set; }
        public string? calledMethod { get; set; }
        public object? methodProperties { get; set; }

    }
}
