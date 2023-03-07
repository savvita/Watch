namespace Watch.DataAccess.UI.Exceptions
{
    public class BasketNotFoundException : Exception
    {
        public BasketNotFoundException() : base("Basket not found")
        {
        }
    }
}
