using Microsoft.Extensions.DependencyInjection;



ServiceCollection services = new ServiceCollection();
services.AddSingleton<IRenderer, Renderer3D>();
services.AddSingleton<ILoader, FileLoader>();
services.AddTransient<Unit>();

ServiceProvider provider = services.BuildServiceProvider();     // <-- DIC instance

//IRenderer renderer = provider.GetRequiredService<IRenderer>();
//renderer.Render();

Unit unit1 = provider.GetRequiredService<Unit>();
unit1.Show();


Unit unit2 = new Unit(new Renderer3D(new FileLoader()));
unit2.Show();


interface IRenderer
{
    public void Render();
}
interface ILoader
{
    public void Load();
}

class Renderer2D : IRenderer
{
    public void Render()
    {
        Console.WriteLine("Renderer2d");
    }
}
class Renderer3D : IRenderer
{
    public ILoader Loader { get; set; }
    public Renderer3D(ILoader loader)
    {
        Loader = loader;
    }

    public void Render()
    {
        Loader.Load();
        Console.WriteLine("Renderer3d");
    }
}
class FileLoader : ILoader
{
    public void Load()
    {
        Console.WriteLine("FileLoader");
    }
}


class Unit
{
    public IRenderer Renderer { get; set; }
    public Unit(IRenderer renderer)
    {
        Renderer = renderer;
    }
    public void Show()
    {
        Renderer.Render();
    }
}






