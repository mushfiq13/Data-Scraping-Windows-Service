using System.ComponentModel.DataAnnotations;

namespace StockExchange.Domain.Entities;

public class Price
{
	/// <summary>
	/// Store milliseconds per 
	/// </summary>
	public ulong Id { get; init; }

	/// <summary>
	/// Refer the corresponding company
	/// </summary>
	[Required]
	public Company Company { get; set; } = null!;

	/// <summary>
	/// Set Last Trading Price  of the day
	/// </summary>
	[Required]
	public decimal LTP { get; set; }

	/// <summary>
	/// Current volume
	/// </summary>
	[Required]
	public int Volume { get; set; }

	/// <summary>
	/// Opening Price of the day
	/// </summary>
	[Required]
	public decimal OpeningPrice { get; set; }

	/// <summary>
	/// High Price of the day
	/// </summary>
	[Required]
	public decimal HighPrice { get; set; }

	/// <summary>
	/// Low Price of the day
	/// </summary>
	[Required]
	public decimal LowPrice { get; set; }

	/// <summary>
	/// Set the object creation time 
	/// </summary>
	[Required]
	public DateTimeOffset Time { get; init; }

	public Price()
	{
		var utcNow = DateTime.UtcNow;
		var _currentTime = new DateTimeOffset(utcNow)
			.ToUnixTimeMilliseconds();

		Id = Convert.ToUInt64(_currentTime);
		Time = utcNow;
	}
}
