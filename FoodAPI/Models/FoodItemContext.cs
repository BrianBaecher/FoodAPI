using Microsoft.EntityFrameworkCore;


namespace FoodAPI.Models
{
	public class FoodItemContext : DbContext
	{
		public FoodItemContext(DbContextOptions<FoodItemContext> options) : base(options) { }

		public DbSet<FoodItem> FoodItems { get; set; }
	}
}
