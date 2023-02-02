namespace Watch.WebApi.Exceptions
{
    public class InternalServerException : Exception
    {
        public InternalServerException() : base("Internal server error")
        {

        }
    }
}
