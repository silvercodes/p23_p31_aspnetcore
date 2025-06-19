// See
UnitBuilder builder = new UnitBuilder();
Unit u = builder.CreateUnit();
u.Show();

// IService
interface IRenderer
{
    public void Render();
}
// Service
class Renderer2D : IRenderer
{
    public void Render()
    {
        Console.WriteLine("Renderer2d");
    }
}
// Service
class Renderer3D : IRenderer
{
    public void Render()
    {
        Console.WriteLine("Renderer3d");
    }
}


// Client
class Unit
{
    public IRenderer Renderer { get; set; }             // <----
    public Unit(IRenderer renderer)
    {
        Renderer = renderer;
    }
    public void Show()
    {
        Renderer.Render();
    }
}
// Injector
class UnitBuilder
{
    public Unit CreateUnit()
    {
        IRenderer renderer = new Renderer2D();
        Unit u = new Unit(renderer);

        return u;
    }
}
