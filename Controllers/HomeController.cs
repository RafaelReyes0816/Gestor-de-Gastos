using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gestor_Gastos.Models;
using Gestor_Gastos.Models.ViewModels;
using Gestor_Gastos.Data;
using Gestor_Gastos.Helpers;

namespace Gestor_Gastos.Controllers;

[Helpers.Authorize("Usuario", "Administrador")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var userId = GetUserId();
        var hoy = DateTime.Today;

        // Calcular totales por período
        var totalGastosDia = await _context.Gastos
            .Where(g => g.UsuarioId == userId && g.FechaGasto.Date == hoy)
            .SumAsync(g => (decimal?)g.Monto) ?? 0;

        // Calcular inicio de semana (lunes)
        var diasDesdeLunes = ((int)hoy.DayOfWeek - (int)DayOfWeek.Monday + 7) % 7;
        var inicioSemana = hoy.AddDays(-diasDesdeLunes);
        var finSemana = inicioSemana.AddDays(7);

        var totalGastosSemana = await _context.Gastos
            .Where(g => g.UsuarioId == userId && g.FechaGasto.Date >= inicioSemana && g.FechaGasto.Date < finSemana)
            .SumAsync(g => (decimal?)g.Monto) ?? 0;

        var totalGastosMes = await _context.Gastos
            .Where(g => g.UsuarioId == userId && g.FechaGasto.Year == hoy.Year && g.FechaGasto.Month == hoy.Month)
            .SumAsync(g => (decimal?)g.Monto) ?? 0;

        var totalGastosAnio = await _context.Gastos
            .Where(g => g.UsuarioId == userId && g.FechaGasto.Year == hoy.Year)
            .SumAsync(g => (decimal?)g.Monto) ?? 0;

        // Estadísticas generales
        var gastos = await _context.Gastos
            .Where(g => g.UsuarioId == userId)
            .ToListAsync();

        var totalGastosRegistrados = gastos.Count;
        var promedioGasto = gastos.Any() ? gastos.Average(g => g.Monto) : 0;
        var gastoMaximo = gastos.Any() ? gastos.Max(g => g.Monto) : 0;
        var gastoMinimo = gastos.Any() ? gastos.Min(g => g.Monto) : 0;

        // Gastos por categoría
        var gastosPorCategoria = await _context.Gastos
            .Where(g => g.UsuarioId == userId)
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

        // Calcular porcentajes
        var totalGeneral = gastosPorCategoria.Sum(g => g.TotalMonto);
        if (totalGeneral > 0)
        {
            foreach (var item in gastosPorCategoria)
            {
                item.Porcentaje = (item.TotalMonto / totalGeneral) * 100;
            }
        }

        // Últimos gastos (últimos 5)
        var ultimosGastos = await _context.Gastos
            .Where(g => g.UsuarioId == userId)
            .Include(g => g.Categoria)
            .OrderByDescending(g => g.FechaGasto)
            .ThenByDescending(g => g.FechaCreacion)
            .Take(5)
            .Select(g => new GastoViewModel
            {
                Id = g.Id,
                CategoriaId = g.CategoriaId,
                Monto = g.Monto,
                Descripcion = g.Descripcion,
                FechaGasto = g.FechaGasto,
                CategoriaNombre = g.Categoria.Nombre,
                CategoriaColor = g.Categoria.Color ?? "#95A5A6"
            })
            .ToListAsync();

        // Gastos por mes (últimos 6 meses)
        var seisMesesAtras = hoy.AddMonths(-6);
        var gastosPorMesRaw = await _context.Gastos
            .Where(g => g.UsuarioId == userId && g.FechaGasto >= seisMesesAtras)
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

        // Formatear nombres de meses en español
        var cultureInfo = new System.Globalization.CultureInfo("es-ES");
        var gastosPorMes = gastosPorMesRaw.Select(g => new GastoPorMes
        {
            Anio = g.Anio,
            Mes = g.Mes,
            NombreMes = new DateTime(g.Anio, g.Mes, 1).ToString("MMMM yyyy", cultureInfo),
            TotalMonto = g.TotalMonto,
            CantidadGastos = g.CantidadGastos
        }).ToList();

        var viewModel = new DashboardViewModel
        {
            TotalGastosDia = totalGastosDia,
            TotalGastosSemana = totalGastosSemana,
            TotalGastosMes = totalGastosMes,
            TotalGastosAnio = totalGastosAnio,
            TotalGastosRegistrados = totalGastosRegistrados,
            PromedioGasto = promedioGasto,
            GastoMaximo = gastoMaximo,
            GastoMinimo = gastoMinimo,
            GastosPorCategoria = gastosPorCategoria,
            UltimosGastos = ultimosGastos,
            GastosPorMes = gastosPorMes
        };

        return View(viewModel);
    }

    private int GetUserId()
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        return userId ?? 0;
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
