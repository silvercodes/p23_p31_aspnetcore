using _20_db_using.Models;
using Microsoft.EntityFrameworkCore;

// dotnet tool install --global dotnet-ef
// dotnet ef migrations add InitialCreate
// dotnet ef database update

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<Db>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<Db>();
    db.Database.Migrate();
}

// Create Todo
app.MapPost("/todos", async (TodoItem todo, Db db) =>
{
    if (string.IsNullOrWhiteSpace(todo.Title))
        return Results.BadRequest("Title is required");

    db.TodoItems.Add(todo);
    await db.SaveChangesAsync();

    return Results.Created($"/todos/{todo.Id}", todo);
});

// Get all todos
app.MapGet("/todos", async (Db db) =>
    await db.TodoItems.ToListAsync());

// Get specific todo
app.MapGet("/todos/{id}", async (int id, Db db) =>
    await db.TodoItems.FindAsync(id) is TodoItem todo
    ? Results.Ok(todo)
    : Results.NotFound());

// Update todo
app.MapPut("/todos/{id}", async (int id, TodoItem inputTodo, Db db) =>
{
    var todo = await db.TodoItems.FindAsync(id);
    if (todo is null)
        return Results.NotFound();

    todo.Title = inputTodo.Title;
    todo.IsCompleted = inputTodo.IsCompleted;

    if (todo.IsCompleted && todo.CompletedAt is null)
        todo.CompletedAt = DateTime.Now;
    else if (!todo.IsCompleted)
        todo.CompletedAt = null;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

// Delete
app.MapDelete("/todos/{id}", async(int id, Db db) =>
{
    if (await db.TodoItems.FindAsync(id) is TodoItem todo)
    {
        db.TodoItems.Remove(todo);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});

// Get all completed todos
app.MapGet("/todos/completed", async (Db db) => 
    await db.TodoItems
        .Where(t => t.IsCompleted)
        .ToListAsync());

// Filtering by word
app.MapGet("/todos/search/{term}", async (string term, Db db) => 
    await db.TodoItems
        .Where(t => t.Title.Contains(term))
        .ToListAsync());

app.Run();
