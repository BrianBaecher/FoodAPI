namespace FoodAPI.Models
{
	using CsvHelper.Configuration.Attributes;
	public class FoodItem
	{
		[Ignore]
		public long Id { get; set; }

		[Name("FOOD NAME")]
		public string? Name { get; set; }

		[Name("SCIENTIFIC NAME")]
		public string? ScientificName { get; set; }

		[Name("GROUP")]
		public string? Group { get; set; }

		[Name("SUB GROUP")]
		public string? SubGroup { get; set; }
	}
}
