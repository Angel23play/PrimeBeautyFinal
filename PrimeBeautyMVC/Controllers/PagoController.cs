using Microsoft.AspNetCore.Mvc;
using PrimeBeautyMVC.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PrimeBeautyMVC.Controllers
{
    public class PagoController : Controller
    {
        private readonly PrimebeautyContext context;
        public PagoController(PrimebeautyContext dbcontext)
        {
            context = dbcontext;
        }
        public async Task<IActionResult> Index()
        {
            var Pagos = await context.Pagos.ToListAsync();
            return View(Pagos);
        }
    }
}
