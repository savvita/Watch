namespace Watch.WebApi.Exceptions
{
    public class BasketNotFoundException : Exception
    {
        public BasketNotFoundException() : base("Basket not found")
        {

        }
    }
}
