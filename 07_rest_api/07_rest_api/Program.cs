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

			case string p when (path.Value?.StartsWith("/products/") ?? false) && method == "GET":
				int? id = ExtractIdFromPath(p);

				if (id.HasValue && products.FirstOrDefault(prod => prod.Id == id) is Product product)
				{
                    await res.WriteAsJsonAsync(product, jsonOptions);
					break;
                }

                res.StatusCode = StatusCodes.Status404NotFound;
                await res.WriteAsync("Product not found");
                
				break;

			case "/products" when method == "POST":

				Product? newProduct = await req.ReadFromJsonAsync<Product>(jsonOptions);

				if (newProduct is null || string.IsNullOrWhiteSpace(newProduct.Name) || newProduct.Price <= 0)
				{
					res.StatusCode = StatusCodes.Status400BadRequest;
                    await res.WriteAsync("Product data is invalid");
					break;
                }

				newProduct.Id = products.Any() ? products.Max(prod => prod.Id) + 1 : 101;
				products.Add(newProduct);

				await SaveProductsAsync(products);
				res.StatusCode = StatusCodes.Status201Created;
				await res.WriteAsJsonAsync(newProduct, jsonOptions);

				break;

            case string p when (path.Value?.StartsWith("/products/") ?? false) && method == "PUT":
                int? updatedId = ExtractIdFromPath(p);

                if (! updatedId.HasValue)
                {
					res.StatusCode = StatusCodes.Status404NotFound;
                    await res.WriteAsync("Invalid Id");
                    break;
                }

				var existingProduct = products.FirstOrDefault(prod => prod.Id == updatedId.Value);
				if (existingProduct is null)
				{
                    res.StatusCode = StatusCodes.Status404NotFound;
                    await res.WriteAsync("Product not found");
                    break;
                }

				var updatedData = await req.ReadFromJsonAsync<Product>(jsonOptions);
                if (updatedData is null || string.IsNullOrWhiteSpace(updatedData.Name) || updatedData.Price <= 0)
                {
                    res.StatusCode = StatusCodes.Status400BadRequest;
                    await res.WriteAsync("Product data is invalid");
                    break;
                }

				existingProduct.Name = updatedData.Name;
				existingProduct.Price = updatedData.Price;
				existingProduct.Stock = updatedData.Stock;
				existingProduct.Category = updatedData.Category;

				await SaveProductsAsync(products);
				res.StatusCode = StatusCodes.Status204NoContent;
				await res.WriteAsJsonAsync(existingProduct, jsonOptions);

                break;

            case string p when (path.Value?.StartsWith("/products/") ?? false) && method == "DELETE":
                int? deletedId = ExtractIdFromPath(p);

                if (!deletedId.HasValue)
                {
                    res.StatusCode = StatusCodes.Status404NotFound;
                    await res.WriteAsync("Invalid Id");
                    break;
                }

                var productToDelete = products.FirstOrDefault(prod => prod.Id == deletedId.Value);
                if (productToDelete is null)
                {
                    res.StatusCode = StatusCodes.Status404NotFound;
                    await res.WriteAsync("Product not found");
                    break;
                }

				products.Remove(productToDelete);
				await SaveProductsAsync(products);

				res.StatusCode = StatusCodes.Status204NoContent;

                break;

			default:
				res.StatusCode = StatusCodes.Status404NotFound;
				await res.WriteAsync("Endpoint not found");
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

int? ExtractIdFromPath(string path)
{
	var segments = path.Split('/');

	if (segments.Length == 3 && int.TryParse(segments[2], out int id))
		return id;

	return null;			// :-(
}

async Task SaveProductsAsync(List<Product> products)
{
	await using var stream = File.Create(dataFilePath);

	await JsonSerializer.SerializeAsync(stream, products, jsonOptions);
}