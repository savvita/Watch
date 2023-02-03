namespace Watch.DataAccess.UI.Exceptions
{
    public class ConflictException : Exception
    {
        public ConflictException() : base("Login is already registered")
        {

        }
    }
}
