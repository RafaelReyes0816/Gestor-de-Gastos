using Gestor_Gastos.Data;
using Gestor_Gastos.Models;
using Gestor_Gastos.Models.ViewModels;
using Gestor_Gastos.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Gestor_Gastos.Controllers
{
    [Helpers.Authorize("Usuario", "Administrador")]
    public class GastosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GastosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Gastos
        public async Task<IActionResult> Index(string periodo = "semanal")
        {
            var userId = GetUserId();
            ViewBag.PeriodoSeleccionado = periodo;

            var gastos = await ObtenerGastosPorPeriodo(userId, periodo);
            var categorias = await _context.Categorias.Where(c => c.Activo).ToListAsync();

            // Pasar datos para el selector de categorías y filtros
            ViewBag.Categorias = categorias;
            return View(gastos);
        }

        // GET: Gastos/Create
        public async Task<IActionResult> Create()
        {
            var categorias = await _context.Categorias.Where(c => c.Activo).ToListAsync();
            ViewBag.Categorias = new SelectList(categorias, "Id", "Nombre");
            return View(new GastoViewModel { FechaGasto = DateTime.Today });
        }

        // POST: Gastos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GastoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var gasto = new Gasto
                {
                    UsuarioId = GetUserId(),
                    CategoriaId = model.CategoriaId,
                    Monto = model.Monto,
                    Descripcion = model.Descripcion?.Trim(),
                    FechaGasto = model.FechaGasto.Date, // Solo fecha, sin hora
                    FechaCreacion = DateTime.Now,
                    FechaModificacion = DateTime.Now
                };

                _context.Gastos.Add(gasto);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Gasto registrado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            var categorias = await _context.Categorias.Where(c => c.Activo).ToListAsync();
            ViewBag.Categorias = new SelectList(categorias, "Id", "Nombre", model.CategoriaId);
            return View(model);
        }

        // GET: Gastos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var gasto = await _context.Gastos.FindAsync(id);
            if (gasto == null || gasto.UsuarioId != GetUserId()) return NotFound();

            var model = new GastoViewModel
            {
                Id = gasto.Id,
                CategoriaId = gasto.CategoriaId,
                Monto = gasto.Monto,
                Descripcion = gasto.Descripcion,
                FechaGasto = gasto.FechaGasto
            };

            var categorias = await _context.Categorias.Where(c => c.Activo).ToListAsync();
            ViewBag.Categorias = new SelectList(categorias, "Id", "Nombre", gasto.CategoriaId);
            return View(model);
        }

        // POST: Gastos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GastoViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var gasto = await _context.Gastos.FindAsync(id);
                if (gasto == null || gasto.UsuarioId != GetUserId()) return NotFound();

                gasto.CategoriaId = model.CategoriaId;
                gasto.Monto = model.Monto;
                gasto.Descripcion = model.Descripcion?.Trim();
                gasto.FechaGasto = model.FechaGasto.Date;
                gasto.FechaModificacion = DateTime.Now;

                try
                {
                    _context.Update(gasto);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Gasto actualizado correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GastoExists(id)) return NotFound();
                    else throw;
                }
            }

            var categorias = await _context.Categorias.Where(c => c.Activo).ToListAsync();
            ViewBag.Categorias = new SelectList(categorias, "Id", "Nombre", model.CategoriaId);
            return View(model);
        }

        // GET: Gastos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var gasto = await _context.Gastos
                .Include(g => g.Categoria)
                .Include(g => g.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (gasto == null || gasto.UsuarioId != GetUserId()) return NotFound();

            return View(gasto);
        }

        // POST: Gastos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gasto = await _context.Gastos.FindAsync(id);
            if (gasto != null && gasto.UsuarioId == GetUserId())
            {
                _context.Gastos.Remove(gasto);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Gasto eliminado correctamente.";
            }
            return RedirectToAction(nameof(Index));
        }

        // 🔍 Métodos auxiliares

        private async Task<List<GastoViewModel>> ObtenerGastosPorPeriodo(int userId, string periodo)
        {
            var hoy = DateTime.Today;

            IQueryable<Gasto> query = _context.Gastos
                .Where(g => g.UsuarioId == userId);

            switch (periodo.ToLower())
            {
                case "diario":
                    query = query.Where(g => g.FechaGasto.Date == hoy);
                    break;
                case "semanal":
                    // Calcular el lunes de la semana actual
                    var diasDesdeLunes = ((int)hoy.DayOfWeek - (int)DayOfWeek.Monday + 7) % 7;
                    var inicioSemana = hoy.AddDays(-diasDesdeLunes);
                    var finSemana = inicioSemana.AddDays(7);
                    query = query.Where(g => g.FechaGasto.Date >= inicioSemana && g.FechaGasto.Date < finSemana);
                    break;
                case "mensual":
                    query = query.Where(g => 
                        g.FechaGasto.Year == hoy.Year && 
                        g.FechaGasto.Month == hoy.Month);
                    break;
                case "anual":
                    query = query.Where(g => g.FechaGasto.Year == hoy.Year);
                    break;
                default:
                    // Por defecto: últimos 30 días
                    query = query.Where(g => g.FechaGasto.Date >= hoy.AddDays(-30));
                    break;
            }

            var gastos = await query
                .Include(g => g.Categoria)
                .OrderByDescending(g => g.FechaGasto)
                .Select(g => new GastoViewModel
                {
                    Id = g.Id,
                    CategoriaId = g.CategoriaId,
                    Monto = g.Monto,
                    Descripcion = g.Descripcion,
                    FechaGasto = g.FechaGasto,
                    CategoriaNombre = g.Categoria.Nombre,
                    CategoriaColor = g.Categoria.Color
                })
                .ToListAsync();

            return gastos;
        }

        private bool GastoExists(int id)
        {
            return _context.Gastos.Any(e => e.Id == id);
        }

        private int GetUserId()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            return userId ?? 0;
        }
    }
}