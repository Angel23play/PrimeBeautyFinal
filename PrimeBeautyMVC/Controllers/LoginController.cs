 using Microsoft.AspNetCore.Mvc;
using PrimeBeautyMVC.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using BCrypt.Net;

namespace PrimeBeautyMVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly PrimebeautyContext _context;

        public LoginController(PrimebeautyContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Registrar()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Registrar(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                usuario.Tipo = "cliente";
                usuario.Contrasena = BCrypt.Net.BCrypt.HashPassword(usuario.Contrasena);
                await _context.Usuarios.AddAsync(usuario);
                await _context.SaveChangesAsync();
                TempData["Mensaje"] = "Usuario registrado correctamente.";
                return RedirectToAction("Login", "Login");
            }
            return View(usuario);
        }
       
        [HttpPost]
        public async Task<IActionResult> Login(Usuario usuario)
        {
            if (string.IsNullOrEmpty(usuario.Email) || string.IsNullOrEmpty(usuario.Contrasena))
            {
                ViewBag.Error = "Debe ingresar usuario y contraseña.";
                return View(usuario);
            }

            var userInDb = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == usuario.Email);

            if (userInDb != null)
            {
                bool isPasswordHashed = userInDb.Contrasena.StartsWith("$2a$")
                                        || userInDb.Contrasena.StartsWith("$2b$")
                                        || userInDb.Contrasena.StartsWith("$2y$");

                if (!isPasswordHashed)
                {
                    // Contraseña almacenada en texto plano o formato viejo
                    if (usuario.Contrasena == userInDb.Contrasena)
                    {
                        // Re-hashear la contraseña con BCrypt y actualizar base de datos
                        userInDb.Contrasena = BCrypt.Net.BCrypt.HashPassword(usuario.Contrasena);
                        _context.Usuarios.Update(userInDb);
                        await _context.SaveChangesAsync();

                        // Login exitoso, continuar...
                    }
                    else
                    {
                        ViewBag.Error = "Usuario o contraseña incorrectos.";
                        return View(usuario);
                    }
                }
                else
                {
                    // Contraseña ya hasheada, verificar con BCrypt
                    if (!BCrypt.Net.BCrypt.Verify(usuario.Contrasena, userInDb.Contrasena))
                    {
                        ViewBag.Error = "Usuario o contraseña incorrectos.";
                        return View(usuario);
                    }
                }

                // Si llega aquí, la autenticación fue exitosa:
                HttpContext.Session.SetString("UserType", userInDb.Tipo);
                TempData["Mensaje"] = $"Bienvenido {userInDb.Email}";

                if (userInDb.Tipo == "cliente")
                    return RedirectToAction("Index", "Home");
                else if (userInDb.Tipo == "admin")
                    return RedirectToAction("Index", "Usuario");
            }

            ViewBag.Error = "Usuario o contraseña incorrectos.";
            return View(usuario);
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["Mensaje"] = "Sesión cerrada.";
            return RedirectToAction("Login", "Login");
        }
    }
}
