using Microsoft.EntityFrameworkCore;
using StockExchange.Domain.Entities;

namespace StockExchange.Persistence.DataAccess;

public interface IStockExchangeDbContext
{
	DbSet<Company> Company { get; }
	DbSet<Price> Price { get; }
}