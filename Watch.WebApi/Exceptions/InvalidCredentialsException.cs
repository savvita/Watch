namespace Watch.WebApi.Exceptions
{
    public class InvalidCredentialsException : Exception
    {
        public InvalidCredentialsException() : base("Username or/and password are invalid")
        {

        }
    }
}
