namespace Watch.DataAccess.UI.Exceptions
{
    public class InactiveUserException : Exception
    {
        public string UserId { get; }
        public InactiveUserException(string userId) : base($"User {userId} is inactive")
        {
            UserId = userId;
        }
    }
}
