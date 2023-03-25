namespace Watch.DataAccess.UI.Exceptions
{
    public class ReviewNotFoundException : Exception
    {
        public int ReviewId { get; }
        public ReviewNotFoundException(int id) : base($"Review with Id {id} not found")
        {
            ReviewId = id;
        }
    }
}
