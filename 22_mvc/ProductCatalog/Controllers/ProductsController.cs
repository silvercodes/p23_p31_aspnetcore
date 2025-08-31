using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Models;

namespace ProductCatalog.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductDbContext db;

        public ProductsController(ProductDbContext db)
        {
            this.db = db;
        }

        // GET /Products
        public async Task<IActionResult> Index()
        {
            return View(await db.Products.ToListAsync());
        }

        // GET /Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // [HttpPost]
        // public Task<IActionResult> Create(Product product)
        // {

        // }

    }
}
