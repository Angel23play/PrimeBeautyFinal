using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrimeBeautyMVC.Models;

namespace PrimeBeautyMVC.Controllers
{
    public class CitasController : Controller
    {

        private readonly PrimebeautyContext context;
        public CitasController(PrimebeautyContext dbcontext)
        {
            context = dbcontext;
        }
        public async Task<IActionResult> Index()
        {
            var Citas = await context.Citas.ToListAsync();
            return View(Citas);
        }

    }
}
