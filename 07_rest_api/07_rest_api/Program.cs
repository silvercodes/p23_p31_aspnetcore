using System.Text.Json;
using _07_rest_api;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var env = app.Environment;

// Конфигурация
const string dataFileName = "products.json";
string dataFilePath = Path.Combine(env.ContentRootPath, "Storage", dataFileName);
var jsonOptions = new JsonSerializerOptions
{
	PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
	WriteIndented = true,
};

// Инициализация хранилища
await InitializeStorageAsync();

app.Run(async context =>
{
    var req = context.Request;
    var res = context.Response;
    var path = req.Path;
    var method = req.Method;

	try
	{
		// GET		/products			Получить список продуктов
		// GET		/products/{id}		Получить конкретный продукт
		// POST		/products			Создать продукт
		// PUT		/products/{id}		Обновить продукт полностью
		// PATCH	/products/{id}		Обновить продукт частично
		// DELETE	/products/{id}		Удаление продукта

		List<Product> products = await LoadProducts();

		switch(path)
		{
			case "/products" when method == "GET":
				await res.WriteAsJsonAsync(products, jsonOptions);
				break;
		}





	}
	catch (Exception ex)
	{
		res.StatusCode = StatusCodes.Status500InternalServerError;

		await res.WriteAsJsonAsync(new {Error = ex.Message}, jsonOptions);
	}
});


app.Run();


async Task InitializeStorageAsync()
{
	if (! File.Exists(dataFilePath))
	{
		await File.WriteAllTextAsync(dataFilePath, JsonSerializer.Serialize(new List<Product>
		{
			new Product {Id = 101, Name = "Laptop", Price = 1000.0m, Stock = 10, Category = "Electronics"},
            new Product {Id = 102, Name = "Smartphone", Price = 300.0m, Stock = 25, Category = "Electronics"},
        }, jsonOptions));
	}
}

async Task<List<Product>> LoadProducts()
{
	await using var fs = File.OpenRead(dataFilePath);

	return await JsonSerializer.DeserializeAsync<List<Product>>(fs, jsonOptions) 
					?? new List<Product>();
}