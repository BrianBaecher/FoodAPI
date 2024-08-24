using FoodAPI.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
//builder.Services.AddDbContext<FoodItemContext>(options => options.UseInMemoryDatabase("FoodItems"));
builder.Services.AddSingleton<FoodCsvService>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/* https://learn.microsoft.com/en-us/azure/azure-sql/database/azure-sql-dotnet-entity-framework-core-quickstart?view=azuresql&tabs=visual-studio%2Cservice-connector%2Cportal
 2. Add the following code to the Program.cs file above the line of code that reads var app = builder.Build();. This code performs the following configurations:

Retrieves the passwordless database connection string from the appsettings.Development.json file for local development, or from the environment variables for hosted production scenarios.

Registers the Entity Framework Core DbContext class with the .NET dependency injection container.
 */

var connection = string.Empty;
if (builder.Environment.IsDevelopment())
{
	builder.Configuration.AddEnvironmentVariables().AddJsonFile("appsettings.Development.json");
	connection = builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING");
}
else
{
	connection = Environment.GetEnvironmentVariable("AZURE_SQL_CONNECTIONSTRING");
}

// db context...
builder.Services.AddDbContext<FoodItemContext>(options => options.UseSqlServer(connection));

var app = builder.Build();

// populating food db 
PopulateDatabase(app);

void PopulateDatabase(WebApplication app)
{
	using (var scope = app.Services.CreateScope())
	{
		var ctx = scope.ServiceProvider.GetRequiredService<FoodItemContext>();
		var csvService = scope.ServiceProvider.GetRequiredService<FoodCsvService>();

		// only populate if empty...
		if (!ctx.FoodItems.Any())
		{
			// load csv data
			var foodItemsList = csvService.LoadFoodItems("generic-food.csv");

			// add items in list to db
			ctx.FoodItems.AddRange(foodItemsList);
			ctx.SaveChanges();
		}
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

/* 
 3. Add the following endpoints to the bottom of the Program.cs file above app.Run() to retrieve and add entities in the database using the PersonDbContext class.
   ** seeing as how i already have a controller defined (FoodItemsController.cs), the gets/posts/etc. are already mapped via the controller... **
 */

app.Run();
