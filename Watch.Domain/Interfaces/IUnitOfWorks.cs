using Microsoft.AspNetCore.Identity;
using Watch.Domain.Models;

namespace Watch.Domain.Interfaces
{
    public interface IUnitOfWorks : IDisposable
    {
        // TODO Basket & Basket Details?
        IOrderDetailRepository OrderDetails { get; }
        IOrderRepository Orders { get; }
        IBrandRepository Brands { get; }
        IUserRepository Users { get; }
        IWatchRepository Watches { get; }
        IOrderStatusRepository OrderStatuses { get; }
        IWaterResistanceRepository WaterResistance { get; }
        IMovementTypeRepository MovementTypes { get; }
        IMaterialRepository Materials { get; }
        IIncrustationTypeRepository IncrustationTypes { get; }
        IGlassTypeRepository GlassTypes { get; }
        IGenderRepository Genders { get; }
        IFunctionRepository Functions { get; }
        IDialTypeRepository DialTypes { get; }
        ICountryRepository Countries { get; }
        IColorRepository Colors { get; }
        ICollectionRepository Collections { get; }
        ICaseShapeRepository CaseShapes { get;  }
        IStyleRepository Styles{ get; }
        IStrapTypeRepository StrapTypes { get; }
        UserManager<UserModel> UserManager { get; }
        RoleManager<IdentityRole> Roles { get; }
    }
}
