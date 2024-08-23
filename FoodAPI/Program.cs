using FoodAPI.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddDbContext<FoodItemContext>(options => options.UseInMemoryDatabase("FoodItems"));
builder.Services.AddSingleton<FoodCsvService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// populating food db 
PopulateDatabase(app);

void PopulateDatabase(WebApplication app)
{
	using (var scope = app.Services.CreateScope())
	{
		var ctx = scope.ServiceProvider.GetRequiredService<FoodItemContext>();
		var csvService = scope.ServiceProvider.GetRequiredService<FoodCsvService>();

		// load csv data
		var foodItemsList = csvService.LoadFoodItems("generic-food.csv");

		// add items in list to db
		ctx.FoodItems.AddRange(foodItemsList);
		ctx.SaveChanges();
	}
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
