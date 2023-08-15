using Autofac;
using StockExchange.Domain.Entities;
using StockExchange.Domain.Repositories;
using StockExchange.Domain.Services;
using StockExchange.Persistence.DataAccess;
using StockExchange.Persistence.Repositories;
using StockExchange.Persistence.Services;

namespace StockExchange.Persistence.Configurations;

public class PersistenceContainer : Module
{
	private readonly StockExchangeConfig _stockExchangeConfig;

	public PersistenceContainer(StockExchangeConfig stockExchangeConfig)
	{
		_stockExchangeConfig = stockExchangeConfig;
	}

	protected override void Load(ContainerBuilder builder)
	{
		builder.RegisterType<StockExchangeDbContext>()
				.As<IStockExchangeDbContext>()
				.WithParameter("stockExchangeConfig", _stockExchangeConfig)
				.InstancePerDependency();

		builder.RegisterType<StockExchangeRepository<Price, ulong>>()
				.As<IAsyncBaseRepository<Price, ulong>>()
				.InstancePerDependency();

		builder.RegisterType<PriceServices>()
				.As<IAsyncPriceServices>()
				.InstancePerDependency();

		builder.RegisterType<CompanyServices>()
				.As<IAsyncCompanyServices>()
				.InstancePerDependency();

		builder.RegisterType<StockExchangeRepository<Company, int>>()
			.As<IAsyncBaseRepository<Company, int>>()
			.InstancePerDependency();
	}
}
