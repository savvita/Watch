using Watch.DataAccess.UI.Interfaces;

namespace Watch.DataAccess.UI.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly UnitOfWorks.UnitOfWorks _db;
        public PaymentRepository(UnitOfWorks.UnitOfWorks db)
        {
            _db = db;
        }

        public async Task<Payment?> CreateAsync(Payment entity)
        {
            entity.Value = entity.Value.Trim();
            var model = await _db.Payments.CreateAsync((PaymentModel)entity);
            return model != null ? new Payment(model) : null;

        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _db.Payments.DeleteAsync(id);
        }

        public async Task<IEnumerable<Payment>> GetAsync()
        {
            return (await _db.Payments.GetAsync()).Select(model => new Payment(model));
        }

        public async Task<Payment?> GetAsync(int id)
        {
            var model = await _db.Payments.GetAsync(id);

            return model != null ? new Payment(model) : null;
        }

        public async Task<Payment> UpdateAsync(Payment entity)
        {
            entity.Value = entity.Value.Trim();
            var model = await _db.Payments.UpdateAsync((PaymentModel)entity);

            return new Payment(model);
        }
    }
}
