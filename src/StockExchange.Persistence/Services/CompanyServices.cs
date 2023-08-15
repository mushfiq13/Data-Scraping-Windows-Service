using StockExchange.Domain.Entities;
using StockExchange.Domain.Repositories;
using StockExchange.Domain.Services;

namespace StockExchange.Persistence.Services;

public class CompanyServices : IAsyncCompanyServices
{
	private readonly IAsyncBaseRepository<Company, int> _repository;

	public CompanyServices(IAsyncBaseRepository<Company, int> repository)
	{
		_repository = repository;
	}

	public async Task<int> CountAsync()
	{
		return await _repository.CountAsync();
	}

	public async Task<IList<Company>?> GetAllAsync()
	{
		return await _repository.GetAllAsync();
	}

	public async Task AddRangeAsync(params Company[] companies)
	{
		await _repository.AddRangeAsync(companies);
		await _repository.CommitAsync();
	}

	public ValueTask DisposeAsync()
	{
		return _repository.DisposeAsync();
	}
}
