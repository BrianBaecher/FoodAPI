namespace FoodAPI.Models
{
	using CsvHelper;
	using System.Globalization;
	using System.IO;

	public class FoodCsvService
	{
		public List<FoodItem> LoadFoodItems(string csvPath)
		{
			using var reader = new StreamReader(csvPath);
			using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
			return csv.GetRecords<FoodItem>().ToList();
		}
	}
}
