using Autofac;
using StockExchange.Domain.Entities;
using StockExchange.Domain.Services;
using StockExchange.Infrastructure.Services;

namespace WebScraping.WindowsService.Utils;

internal class CompanyProcessor
{
	private readonly ILogger<CompanyProcessor> _logger;
	private readonly ILifetimeScope _scope;

	public CompanyProcessor(
		ILogger<CompanyProcessor> logger,
		ILifetimeScope scope)
	{
		_logger = logger;
		_scope = scope;
	}

	public async Task InitAsync()
	{
		try {
			var companyServices = _scope.Resolve<IAsyncCompanyServices>();

			if (await companyServices.CountAsync() is int totalCompany && totalCompany > 0) {
				_logger.LogInformation(
						"Db contains {Total} companies.", totalCompany);

				return;
			}

			var scrapingConfig = Helper.GetWebScrapingConfig(_scope);
			var scrapingServices = new StockExchangeScrapingServices(scrapingConfig.FullUri);
			var companies = new List<Company>();

			await foreach (var c in scrapingServices.FetchCompanyListFromWebAsync(
				scrapingConfig.Company)) {
				companies.Add(c);
			}

			_logger.LogInformation("Initializing companies.");

			await companyServices.AddRangeAsync(companies.ToArray());

			_logger.LogInformation("Initialization completed.");
		}
		catch (Exception ex) {
			_logger.LogError(ex.Message, "Couldn't insert companies.");
		}
	}

	public async Task<Dictionary<string, Company>> GetCompaniesAsync()
	{
		var companyServices = _scope.Resolve<IAsyncCompanyServices>();
		var companies = await companyServices.GetAllAsync();
		var companiesList = new Dictionary<string, Company>();

		foreach (var c in companies!) {
			companiesList.TryAdd(c.StockCodeName, c);
		}

		return companiesList;
	}
}
