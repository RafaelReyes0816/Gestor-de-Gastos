using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gestor_Gastos.Data;
using Gestor_Gastos.Models;
using Gestor_Gastos.Models.ViewModels;

namespace Gestor_Gastos.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Account/Login
        public IActionResult Login()
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Buscar usuario por username
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Username == model.Username && u.Activo);

            if (usuario == null)
            {
                ModelState.AddModelError("", "Usuario o contraseña incorrectos");
                return View(model);
            }

            // Verificar que el hash tenga el formato correcto de BCrypt
            bool passwordValid = false;
            try
            {
                // Verificar que el hash empiece con $2a$, $2b$ o $2y$ (formatos BCrypt válidos)
                if (string.IsNullOrEmpty(usuario.Password) || 
                    (!usuario.Password.StartsWith("$2a$") && 
                     !usuario.Password.StartsWith("$2b$") && 
                     !usuario.Password.StartsWith("$2y$")))
                {
                    // El hash no tiene formato BCrypt válido
                    ModelState.AddModelError("", "Error en la configuración de la contraseña. Contacte al administrador.");
                    return View(model);
                }

                // Verificar contraseña hasheada con BCrypt
                passwordValid = BCrypt.Net.BCrypt.Verify(model.Password, usuario.Password);
            }
            catch (BCrypt.Net.SaltParseException)
            {
                // El hash no es válido (posiblemente está corrupto o en formato incorrecto)
                ModelState.AddModelError("", "Error en la configuración de la contraseña. Contacte al administrador.");
                return View(model);
            }
            catch (Exception)
            {
                // Cualquier otro error en la verificación
                ModelState.AddModelError("", "Error al verificar la contraseña. Intente nuevamente.");
                return View(model);
            }

            if (!passwordValid)
            {
                ModelState.AddModelError("", "Usuario o contraseña incorrectos");
                return View(model);
            }

            // Crear sesión
            HttpContext.Session.SetInt32("UserId", usuario.Id);
            HttpContext.Session.SetString("Username", usuario.Username);
            HttpContext.Session.SetString("Rol", usuario.Rol);
            HttpContext.Session.SetString("NombreCompleto", $"{usuario.Nombre} {usuario.Apellido}");

            // Redirigir según el rol
            if (usuario.Rol == "Administrador")
            {
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }

        // GET: Account/AccessDenied
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}

