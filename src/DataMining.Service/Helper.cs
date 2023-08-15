using Autofac;
using StockExchange.Infrastructure.Configurations;

namespace WebScraping.WindowsService;

internal static class Helper
{
	public static WebScrapingConfig GetWebScrapingConfig(
		ILifetimeScope scope)
	{
		var scrapingConfig = scope.Resolve<WebScrapingConfig>();
		var configuration = scope.Resolve<IConfiguration>();

		configuration.GetSection("WebScraping").Bind(scrapingConfig);

		return scrapingConfig;
	}
}
