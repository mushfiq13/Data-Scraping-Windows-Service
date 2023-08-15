using Autofac;
using Autofac.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using StockExchange.Infrastructure.Configurations;
using StockExchange.Infrastructure.Services;
using StockExchange.Persistence.Configurations;
using WebScraping.WindowsService;
using WebScraping.WindowsService.Utils;

IConfiguration configuration = new ConfigurationBuilder()
	.AddJsonFile("appsettings.json", false, true)
	.AddJsonFile($"appsettings.{Environments.Development}.json", true, true)
	.Build();
var stockExchangeConfig = new StockExchangeConfig();

configuration
	.GetSection(nameof(stockExchangeConfig))
	.Bind(stockExchangeConfig);

Log.Logger = new LoggerConfiguration()
	.MinimumLevel.Debug()
	.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
	.Enrich.FromLogContext()
	.WriteTo.Console(
				outputTemplate: "[{Timestamp:dd-MM-yyyy HH:mm} {Level:u3}] {Username} {Message:lj}{NewLine}{Exception}")
	.WriteTo.File(
		path: "Logs/log-.log",
		rollingInterval: RollingInterval.Day,
		outputTemplate: "[{Timestamp:dd-MM-yyyy HH:mm} {Level:u3}] {Username} {Message:lj}{NewLine}{Exception}")
	.ReadFrom.Configuration(configuration)
	.CreateLogger();

var host = Host.CreateDefaultBuilder(args)
	.UseSerilog()
	.UseServiceProviderFactory(new AutofacServiceProviderFactory())
	.ConfigureContainer<Autofac.ContainerBuilder>((ctx, b)
		=> b.RegisterModule(new PersistenceContainer(stockExchangeConfig)))
	.ConfigureAppConfiguration((_, config) =>
	{
		config.AddConfiguration(configuration);
	})
	.ConfigureServices(services =>
		{
			services
				.AddHostedService<ScraperService>()
				.AddSingleton<CompanyProcessor>()
				.AddSingleton<PriceProcessor>()
				.AddSingleton<StaticWebScraper>()
				.AddSingleton<WebScrapingConfig>();
		})
	.UseWindowsService()
	.Build();

await host.RunAsync();
