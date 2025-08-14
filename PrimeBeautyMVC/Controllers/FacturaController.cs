using Microsoft.AspNetCore.Mvc;
using PrimeBeautyMVC.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PrimeBeautyMVC.Controllers
{
    public class FacturaController : Controller
    {
        private readonly PrimebeautyContext context;
        public FacturaController(PrimebeautyContext dbcontext)
        {
            context = dbcontext;
        }
        public async Task<IActionResult> Index()
        {
            var Facturas = await context.Facturas.ToListAsync();
            return View(Facturas);
        }
    }
}
