﻿using Watch.Domain.Models;

namespace Watch.Domain.Interfaces
{
    public interface IBrandRepository : IGenericRepository<BrandModel>
    {
        Task<List<SaleModel>> GetSalesAsync();
    }
}
