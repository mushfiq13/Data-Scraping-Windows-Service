using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using StockExchange.Domain.Repositories;
using StockExchange.Persistence.DataAccess;

namespace StockExchange.Persistence.Repositories;

public class StockExchangeRepository<TEntity, TKey> :
		IAsyncBaseRepository<TEntity, TKey>
								where TEntity : class
								where TKey : struct
{
	private readonly DbSet<TEntity> _entitySet;
	private readonly DbContext _context;

	/// <summary>
	/// Receive a db context object.
	/// </summary>
	/// <param name="context"></param>
	public StockExchangeRepository(
		IStockExchangeDbContext context)
	{
		_context = (context as StockExchangeDbContext)!;
		_entitySet = _context.Set<TEntity>();
	}

	public virtual async Task<IList<TEntity>?> GetAllAsync()
	{
		return await _entitySet
				.Select(entity => entity)
				.ToListAsync();
	}

	public virtual async Task AddRangeAsync(params TEntity[] entities)
	{
		try {
			_context.ChangeTracker.AutoDetectChangesEnabled = false;

			await _context.BulkInsertAsync(entities);
		}
		finally {
			_context.ChangeTracker.AutoDetectChangesEnabled = true;
		}
	}

	public async Task<int> CountAsync()
	{
		return await _entitySet.CountAsync();
	}

	public async Task CommitAsync()
	{
		await _context.SaveChangesAsync();
	}

	public ValueTask DisposeAsync()
	{
		return _context.DisposeAsync();
	}
}
