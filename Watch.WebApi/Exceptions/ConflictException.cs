namespace Watch.WebApi.Exceptions
{
    public class ConflictException : Exception
    {
        public ConflictException() : base("Login is already registered")
        {

        }
    }
}
