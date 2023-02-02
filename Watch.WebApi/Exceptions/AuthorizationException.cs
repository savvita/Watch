namespace Watch.WebApi.Exceptions
{
    public class AuthorizationException : Exception
    {
        public AuthorizationException() : base("Authorization failed")
        {

        }
    }
}
