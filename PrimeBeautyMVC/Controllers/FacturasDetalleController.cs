using Microsoft.AspNetCore.Mvc;
using PrimeBeautyMVC.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace PrimeBeautyMVC.Controllers
{
    public class FacturasDetalleController : Controller
    {
        private readonly PrimebeautyContext context;
        public FacturasDetalleController(PrimebeautyContext dbcontext)
        {
            context = dbcontext;
        }
        public async Task<IActionResult> Index()
        {
            var Facturas = await context.FacturasDetalles.ToListAsync();
            return View(Facturas);
        }
    }
}
