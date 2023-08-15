using StockExchange.Domain.Entities;
using StockExchange.Domain.Repositories;
using StockExchange.Domain.Services;

namespace StockExchange.Persistence.Services;

public class PriceServices : IAsyncPriceServices
{
	private readonly IAsyncBaseRepository<Price, ulong> _repository;

	public PriceServices(IAsyncBaseRepository<Price, ulong> repository)
	{
		_repository = repository;
	}

	public async Task AddRangeAsync(params Price[] prices)
	{
		try {
			await _repository.AddRangeAsync(prices);
			await _repository.CommitAsync();
		}
		catch {
			throw new Exception("Couldn't add given price");
		}
	}

	public ValueTask DisposeAsync()
	{
		return _repository.DisposeAsync();
	}
}
