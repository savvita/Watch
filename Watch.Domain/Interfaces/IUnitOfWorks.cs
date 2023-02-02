namespace Watch.Domain.Interfaces
{
    public interface IUnitOfWorks : IDisposable
    {
        ICategoryRepository Categories { get; }
        IOrderDetailRepository OrderDetails { get; }
        IOrderRepository Orders { get; }
        IProducerRepository Producers { get; }
        IUserRepository Users { get; }
        IWatchRepository Watches { get; }
        IOrderStatusRepository OrderStatuses { get; }
    }
}
