using Autofac;
using StockExchange.Domain.Entities;
using StockExchange.Domain.Services;
using StockExchange.Infrastructure.Services;

namespace WebScraping.WindowsService.Utils;

internal class PriceProcessor
{
	private readonly ILogger<PriceProcessor> _logger;
	private readonly ILifetimeScope _scope;

	public PriceProcessor(
			ILogger<PriceProcessor> logger,
			ILifetimeScope scope)
	{
		_logger = logger;
		_scope = scope;
	}

	public async Task ProcessAsync(
		Dictionary<string, Company> companies)
	{
		try {
			var config = Helper.GetWebScrapingConfig(_scope);
			var scrapingServices = new StockExchangeScrapingServices(config.FullUri);
			var prices = await scrapingServices.FetchCurrentPricesFromWebAsync(
				config.Price, companies);
			var services = _scope.Resolve<IAsyncPriceServices>();

			await services.AddRangeAsync(prices.ToArray());

			_logger.LogInformation("Current prices list has been added at {Now}", DateTime.Now);

			await services.DisposeAsync();
		}
		catch (Exception ex) {
			_logger.LogError(ex, ex.Message);
		}
	}
}
