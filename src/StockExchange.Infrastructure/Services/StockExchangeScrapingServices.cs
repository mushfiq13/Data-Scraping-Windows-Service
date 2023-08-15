using HtmlAgilityPack;
using StockExchange.Domain.Entities;
using StockExchange.Infrastructure.Configurations;

namespace StockExchange.Infrastructure.Services;

public class StockExchangeScrapingServices
{
	private readonly StaticWebScraper _scraper;

	public StockExchangeScrapingServices(string fullUri)
	{
		_scraper = new StaticWebScraper(new Uri(fullUri));
	}

	public async Task<HtmlDocument> LoadDocumentAsync()
	{
		return await _scraper.GetDocumentAsync();
	}

	public async IAsyncEnumerable<Company> FetchCompanyListFromWebAsync(
		CompanyConfig config)
	{
		var doc = await LoadDocumentAsync();
		var nodeCollection = await _scraper.GetNodesAsync(
			doc, config.XPath);

		foreach (var node in nodeCollection) {
			var stockCode = node.InnerText;

			yield return new Company { StockCodeName = stockCode! };
		}
	}

	public async Task<bool> CheckMarketStatusIsOpenAsync(
		string xPath = "//*[@class=\"market_status\"]/div/span")
	{
		var doc = await LoadDocumentAsync();
		var marketStatusNode = await _scraper.GetSingleNodeAsync(
			doc, xPath);
		var marketStatus = marketStatusNode.InnerText;

		return marketStatus.Contains("Closed")
			? false : true;
	}

	public async Task<IList<Price>> FetchCurrentPricesFromWebAsync(
		PriceConfig config,
		Dictionary<string, Company> companies)
	{
		var doc = await LoadDocumentAsync();
		var collection = await _scraper.GetNodesAsync(doc, config.XPath);
		var prices = new List<Price>();

		foreach (var row in collection) {
			var columns = row.ChildNodes
				.Where(c => c.NodeType == HtmlNodeType.Element)
				.Select(c => c.InnerText)
			.ToList();

			companies.TryGetValue(columns.ElementAt(1), out var company);

			if (company is null) {
				continue;
			}

			prices.Add(new Price
			{
				Company = company,
				LTP = Convert.ToDecimal(columns.ElementAt(2)),
				OpeningPrice = Convert.ToDecimal(columns.ElementAt(3)),
				HighPrice = Convert.ToDecimal(columns.ElementAt(4)),
				LowPrice = Convert.ToDecimal(columns.ElementAt(5)),
				Volume = int.Parse(columns.Last())
			});
		}

		return prices;
	}
}
