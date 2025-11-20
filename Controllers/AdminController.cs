using Gestor_Gastos.Data;
using Gestor_Gastos.Helpers;
using Gestor_Gastos.Models;
using Gestor_Gastos.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gestor_Gastos.Controllers
{
    [Authorize("Administrador")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var totalUsuarios = await _context.Usuarios.CountAsync();
            var usuariosActivos = await _context.Usuarios.CountAsync(u => u.Activo);
            var totalGastos = await _context.Gastos.CountAsync();
            var totalMonto = await _context.Gastos.SumAsync(g => (decimal?)g.Monto) ?? 0;

            var gastosPorCategoria = await _context.Gastos
                .Include(g => g.Categoria)
                .GroupBy(g => new { g.CategoriaId, g.Categoria.Nombre, g.Categoria.Color })
                .Select(g => new GastoPorCategoria
                {
                    CategoriaId = g.Key.CategoriaId,
                    CategoriaNombre = g.Key.Nombre,
                    CategoriaColor = g.Key.Color ?? "#95A5A6",
                    TotalMonto = g.Sum(x => x.Monto),
                    CantidadGastos = g.Count()
                })
                .OrderByDescending(g => g.TotalMonto)
                .ToListAsync();

            var totalGeneral = gastosPorCategoria.Sum(g => g.TotalMonto);
            if (totalGeneral > 0)
            {
                foreach (var item in gastosPorCategoria)
                {
                    item.Porcentaje = (item.TotalMonto / totalGeneral) * 100;
                }
            }

            var gastosPorMesRaw = await _context.Gastos
                .GroupBy(g => new { Anio = g.FechaGasto.Year, Mes = g.FechaGasto.Month })
                .Select(g => new
                {
                    Anio = g.Key.Anio,
                    Mes = g.Key.Mes,
                    TotalMonto = g.Sum(x => x.Monto),
                    CantidadGastos = g.Count()
                })
                .OrderBy(g => g.Anio)
                .ThenBy(g => g.Mes)
                .ToListAsync();

            var cultureInfo = new System.Globalization.CultureInfo("es-ES");
            var gastosPorMes = gastosPorMesRaw.Select(g => new GastoPorMes
            {
                Anio = g.Anio,
                Mes = g.Mes,
                NombreMes = new DateTime(g.Anio, g.Mes, 1).ToString("MMMM yyyy", cultureInfo),
                TotalMonto = g.TotalMonto,
                CantidadGastos = g.CantidadGastos
            }).ToList();

            var usuariosResumen = await _context.Usuarios
                .Select(u => new AdminUsuarioResumen
                {
                    UsuarioId = u.Id,
                    Username = u.Username,
                    NombreCompleto = u.Nombre + " " + u.Apellido,
                    Rol = u.Rol,
                    Activo = u.Activo,
                    CantidadGastos = u.Gastos.Count,
                    TotalMonto = u.Gastos.Sum(g => (decimal?)g.Monto) ?? 0
                })
                .ToListAsync();

            foreach (var u in usuariosResumen)
            {
                if (u.CantidadGastos > 0)
                {
                    u.PromedioGasto = u.TotalMonto / u.CantidadGastos;
                }
            }

            var viewModel = new AdminDashboardViewModel
            {
                TotalUsuarios = totalUsuarios,
                UsuariosActivos = usuariosActivos,
                TotalGastos = totalGastos,
                TotalMontoGastos = totalMonto,
                GastosPorCategoria = gastosPorCategoria,
                GastosPorMes = gastosPorMes,
                UsuariosResumen = usuariosResumen
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Usuarios()
        {
            var usuarios = await _context.Usuarios
                .OrderBy(u => u.Rol)
                .ThenBy(u => u.Apellido)
                .ThenBy(u => u.Nombre)
                .ToListAsync();

            return View(usuarios);
        }

        public async Task<IActionResult> DetalleUsuario(int id)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
            {
                return NotFound();
            }

            var gastos = await _context.Gastos
                .Where(g => g.UsuarioId == id)
                .Include(g => g.Categoria)
                .OrderByDescending(g => g.FechaGasto)
                .ToListAsync();

            var totalGastos = gastos.Count;
            var totalMonto = gastos.Sum(g => g.Monto);
            var promedio = totalGastos > 0 ? gastos.Average(g => g.Monto) : 0;

            var gastosPorCategoria = gastos
                .GroupBy(g => new { g.CategoriaId, g.Categoria.Nombre, g.Categoria.Color })
                .Select(g => new GastoPorCategoria
                {
                    CategoriaId = g.Key.CategoriaId,
                    CategoriaNombre = g.Key.Nombre,
                    CategoriaColor = g.Key.Color ?? "#95A5A6",
                    TotalMonto = g.Sum(x => x.Monto),
                    CantidadGastos = g.Count()
                })
                .OrderByDescending(g => g.TotalMonto)
                .ToList();

            var totalGeneral = gastosPorCategoria.Sum(g => g.TotalMonto);
            if (totalGeneral > 0)
            {
                foreach (var item in gastosPorCategoria)
                {
                    item.Porcentaje = (item.TotalMonto / totalGeneral) * 100;
                }
            }

            var detalleViewModel = new UsuarioDetalleViewModel
            {
                Usuario = usuario,
                Gastos = gastos,
                TotalGastos = totalGastos,
                TotalMonto = totalMonto,
                PromedioGasto = promedio,
                GastosPorCategoria = gastosPorCategoria
            };

            return View(detalleViewModel);
        }
    }
}
