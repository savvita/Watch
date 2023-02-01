﻿using Watch.DataAccess.UI.Models;

namespace Watch.DataAccess.UI.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<IEnumerable<Order>> GetByUserIdAsync(string userId);
        Task<Order?> CreateAsync(Basket basket);
    }
}