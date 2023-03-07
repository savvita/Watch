namespace Watch.DataAccess.UI.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public string UserId { get; }

        public UserNotFoundException() : base("User not found")
        {
            UserId = String.Empty;
        }
        public UserNotFoundException(string userId) : base($"User {userId} not found")
        {
            UserId = userId;
        }
    }
}
