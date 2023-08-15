using System.ComponentModel.DataAnnotations;

namespace StockExchange.Domain.Entities;

public class Company
{
	/// <summary>
	/// Set company identity
	/// </summary>
	public int Id { get; set; }

	/// <summary>
	/// Set the stock code name e.g. 1JANATAMF
	/// </summary>
	[Required]
	[StringLength(32)]
	public string StockCodeName { get; set; } = null!;
}
