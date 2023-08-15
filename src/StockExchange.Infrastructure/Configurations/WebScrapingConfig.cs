namespace StockExchange.Infrastructure.Configurations;

public class WebScrapingConfig
{
    public string FullUri { get; set; } = null!;
    public PriceConfig Price { get; set; } = null!;
    public CompanyConfig Company { get; set; } = null!;
}
