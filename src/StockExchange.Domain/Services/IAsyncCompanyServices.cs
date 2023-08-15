using StockExchange.Domain.Entities;

namespace StockExchange.Domain.Services;

public interface IAsyncCompanyServices : IAsyncDisposable
{
	Task AddRangeAsync(params Company[] companies);
	Task<int> CountAsync();
	Task<IList<Company>?> GetAllAsync();
}