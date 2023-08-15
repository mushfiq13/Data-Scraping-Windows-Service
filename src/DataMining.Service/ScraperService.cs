using Autofac;
using StockExchange.Domain.Entities;
using StockExchange.Infrastructure.Services;
using WebScraping.WindowsService.Utils;

namespace WebScraping.WindowsService;

public class ScraperService : BackgroundService
{
	private readonly ILogger<ScraperService> _logger;
	private readonly ILifetimeScope _scope;
	private Dictionary<string, Company> _companies = null!;

	public ScraperService(
		ILogger<ScraperService> logger,
		ILifetimeScope scope)
	{
		_logger = logger;
		_scope = scope;
	}

	protected override async Task ExecuteAsync(CancellationToken cancellationToken)
	{
		try {
			var companyProcessor = _scope.Resolve<CompanyProcessor>();
			await companyProcessor.InitAsync();
			_companies = await companyProcessor.GetCompaniesAsync();

			var config = Helper.GetWebScrapingConfig(_scope);
			var scrapingServices = new StockExchangeScrapingServices(config.FullUri);

			while (cancellationToken.IsCancellationRequested is false) {
				var isOpen = await scrapingServices.CheckMarketStatusIsOpenAsync();

				if (isOpen == false) {
					_logger.LogInformation(
					"Retrieving current prices has been stopped as market is closed {Now}", DateTime.UtcNow);
					_logger.LogInformation("Retry after 1 day.");

					await Task.Delay(TimeSpan.FromDays(1), cancellationToken);

					continue;
				}

				var prices = await scrapingServices.FetchCurrentPricesFromWebAsync(
					config.Price, _companies);
				var priceProcessor = _scope.Resolve<PriceProcessor>();
				await priceProcessor.ProcessAsync(_companies);

				await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken);
			}
		}
		catch (Exception ex) {
			_logger.LogError(ex, ex.Message);
		}
	}
}
