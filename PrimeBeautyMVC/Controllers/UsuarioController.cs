using Microsoft.AspNetCore.Mvc;
using PrimeBeautyMVC.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace PrimeBeautyMVC.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly PrimebeautyContext context;
        public UsuarioController(PrimebeautyContext dbcontext)
        {
            context = dbcontext;
        }

        // GET: /Usuario/
        public async Task<IActionResult> Index()
        {
            var usuarios = await context.Usuarios.ToListAsync();
            return View(usuarios);
        }

        // GET: /Usuario/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Usuario/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                context.Usuarios.Add(usuario);
                await context.SaveChangesAsync();
                TempData["SuccessMessage"] = "✅ Usuario creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // GET: /Usuario/Edit/5
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
                return NotFound();

            var usuario = await context.Usuarios.FindAsync(id);
            if (usuario == null)
                return NotFound();

            return View(usuario);
        }

        // POST: /Usuario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Usuario usuario)
        {
            if (id != usuario.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    context.Update(usuario);
                    await context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "✏️ Usuario actualizado exitosamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // GET: /Usuario/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var usuario = await context.Usuarios
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
                return NotFound();

            return View(usuario);
        }

        // POST: /Usuario/DeleteConfirmed/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                context.Usuarios.Remove(usuario);
                await context.SaveChangesAsync();
                TempData["SuccessMessage"] = "🗑️ Usuario eliminado exitosamente.";
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: /Usuario/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var usuario = await context.Usuarios
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
                return NotFound();

            return View(usuario);
        }

        private bool UsuarioExists(int id)
        {
            return context.Usuarios.Any(u => u.Id == id);
        }
    }
}
