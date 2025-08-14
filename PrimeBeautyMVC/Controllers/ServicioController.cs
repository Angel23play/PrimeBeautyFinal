using Microsoft.AspNetCore.Mvc;
using PrimeBeautyMVC.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PrimeBeautyMVC.Controllers
{
    public class ServicioController : Controller
    {
        private readonly PrimebeautyContext context;
        public ServicioController(PrimebeautyContext dbcontext)
        {
            context = dbcontext;
        }
        public async Task<IActionResult> Index()
        {
            var Servicios = await context.Servicios.ToListAsync();
            return View(Servicios);
        }
    }
}
