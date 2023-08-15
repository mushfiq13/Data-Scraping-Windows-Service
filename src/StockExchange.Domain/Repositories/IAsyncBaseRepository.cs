namespace StockExchange.Domain.Repositories;

public interface IAsyncBaseRepository<TEntity, TKey> : IAsyncDisposable
	where TEntity : class
	where TKey : struct
{
	/// <summary>
	/// Retrieve all entities. For an invalid operation throw an 
	/// exception.
	/// </summary>
	/// <returns>TEntity's List</returns>
	Task<IList<TEntity>?> GetAllAsync();

	/// <summary>
	/// Insert all entities at once.
	/// </summary>
	/// <param name="entities"></param>
	/// <returns></returns>
	Task AddRangeAsync(params TEntity[] entities);

	Task CommitAsync();
	Task<int> CountAsync();
}
