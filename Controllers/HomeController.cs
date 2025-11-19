using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Gestor_Gastos.Models;
using Gestor_Gastos.Helpers;

namespace Gestor_Gastos.Controllers;

[Helpers.Authorize("Usuario", "Administrador")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
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
