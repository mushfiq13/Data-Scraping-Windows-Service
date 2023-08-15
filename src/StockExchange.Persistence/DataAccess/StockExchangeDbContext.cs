using Microsoft.EntityFrameworkCore;
using StockExchange.Domain.Entities;
using StockExchange.Persistence.Configurations;

namespace StockExchange.Persistence.DataAccess;

public class StockExchangeDbContext :
		DbContext,
		IStockExchangeDbContext
{
	private readonly StockExchangeConfig _stockExchangeConfig;

	public StockExchangeDbContext() { }

	public StockExchangeDbContext(StockExchangeConfig stockExchangeConfig)
	{
		_stockExchangeConfig = stockExchangeConfig;
	}

	protected override void OnConfiguring(DbContextOptionsBuilder builder)
	{
		builder
			.UseSqlServer(_stockExchangeConfig?.ConnectionString);
	}

	public DbSet<Company> Company => Set<Company>();
	public DbSet<Price> Price => Set<Price>();
}
