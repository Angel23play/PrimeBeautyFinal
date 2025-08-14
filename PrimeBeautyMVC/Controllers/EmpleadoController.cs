using Microsoft.AspNetCore.Mvc;
using PrimeBeautyMVC.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PrimeBeautyMVC.Controllers
{
    public class EmpleadoController : Controller
    {
        private readonly PrimebeautyContext context;
        public EmpleadoController(PrimebeautyContext dbcontext)
        {
            context = dbcontext;
        }

        // Get /Empleado
        public async Task<IActionResult> Index()
        {
            var Empleados = await context.Empleados.ToListAsync();
            return View(Empleados);
        }

        // GET: /Empleado/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Empleado/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Empleado Empleado)
        {
            if (ModelState.IsValid)
            {
                context.Empleados.Add(Empleado);
                await context.SaveChangesAsync();
                TempData["SuccessMessage"] = "✅ Usuario creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            return View(Empleado);
        }

        // GET: /Empleado/Edit/5
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
                return NotFound();

            var Empleado = await context.Empleados.FindAsync(id);
            if (Empleado == null)
                return NotFound();

            return View(Empleado);
        }

        // POST: /Empleado/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Empleado Empleado)
        {
            if (id != Empleado.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    context.Update(Empleado);
                    await context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "✏️ Usuario actualizado exitosamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpleadoExists(Empleado.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(Empleado);
        }

        // GET: /Empleado/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var Empleado = await context.Empleados.FirstOrDefaultAsync(u => u.Id == id);

            if (Empleado == null)
                return NotFound();

            return View(Empleado);
        }

        // POST: /Empleado/DeleteConfirmed/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Empleado = await context.Empleados.FindAsync(id);
            if (Empleado != null)
            {
                context.Empleados.Remove(Empleado);
                await context.SaveChangesAsync();
                TempData["SuccessMessage"] = "🗑️ Usuario eliminado exitosamente.";
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: /Empleado/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var Empleado = await context.Empleados
                .FirstOrDefaultAsync(u => u.Id == id);

            if (Empleado == null)
                return NotFound();

            return View(Empleado);
        }

        private bool EmpleadoExists(int id)
        {
            return context.Empleados.Any(u => u.Id == id);
        }
    }
}




