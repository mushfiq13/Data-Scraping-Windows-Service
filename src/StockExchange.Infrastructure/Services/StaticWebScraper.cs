using HtmlAgilityPack;

namespace StockExchange.Infrastructure.Services;

/// <summary>
/// Download static HTML pages, parse them, and make it possible 
/// to extract the required data from these pages.
/// 
/// It's possible to read and parse files from local 
/// files, HTML strings, any URL, or even a browser. 
/// </summary>
public class StaticWebScraper
{
	private readonly Uri _baseUri;

	public StaticWebScraper(Uri baseUri)
	{
		_baseUri = baseUri;
	}

	public async Task<HtmlDocument> GetDocumentAsync(string? relativeUrl = null)
	{
		try {
			var path = new Uri(_baseUri, relativeUrl);
			var web = new HtmlWeb();
			var doc = await web.LoadFromWebAsync(path.AbsoluteUri);

			return doc;
		}
		catch (Exception ex) {
			Console.WriteLine(ex.Message);
		}

		return null!;
	}

	public async Task<HtmlNode> GetSingleNodeAsync(
		HtmlDocument doc,
		string xPath)
	{
		return await Task.FromResult(
			doc.DocumentNode.SelectSingleNode(xPath));
	}

	/// <summary>
	/// Parse the HTML and get selected node links.
	/// </summary>
	/// <param name="doc"></param>
	/// <param name="xPath"></param>
	/// <returns></returns>
	public async Task<HtmlNodeCollection> GetNodesAsync(
		HtmlDocument doc,
		string xPath)
	{
		return await Task.FromResult(
			doc.DocumentNode.SelectNodes(xPath));
	}

	public async Task<HtmlNodeCollection> GetNodesAsync(
		HtmlNode doc,
		string xPath)
	{
		return await Task.FromResult(
			doc.SelectNodes(xPath));
	}

	public async Task<IList<string>> GetInnerTextAsync(
		HtmlNodeCollection collection)
	{
		var result = new List<string>();

		foreach (var node in collection) {
			result.Add(node.InnerText);
		}

		return await Task.FromResult(result);
	}

	public async IAsyncEnumerable<string> GetAbsoluteUrisAsync(
		HtmlNodeCollection linkNodes)
	{
		foreach (var node in linkNodes) {
			var url = new Uri(
				_baseUri,
				node?.Attributes["href"]?.Value).AbsoluteUri;

			yield return await Task.FromResult(url);
		}
	}
}
