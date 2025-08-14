using Microsoft.AspNetCore.Mvc;
using PrimeBeautyMVC.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PrimeBeautyMVC.Controllers
{
    public class ProductoController : Controller
    {
        private readonly PrimebeautyContext context;
        public ProductoController(PrimebeautyContext dbcontext)
        {
            context = dbcontext;
        }
        public async Task<IActionResult> Index()
        {
            var Productos = await context.Productos.ToListAsync();
            return View(Productos);
        }
    }
}
