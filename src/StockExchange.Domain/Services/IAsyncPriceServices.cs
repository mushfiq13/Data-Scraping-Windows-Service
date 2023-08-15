using StockExchange.Domain.Entities;

namespace StockExchange.Domain.Services;

public interface IAsyncPriceServices : IAsyncDisposable
{
    Task AddRangeAsync(params Price[] prices);
}