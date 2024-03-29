﻿using Watch.Domain.Models;

namespace Watch.Domain.Interfaces
{
    public interface IDialTypeRepository : IGenericRepository<DialTypeModel>
    {
        Task<List<SaleModel>> GetSalesAsync();
    }
}
