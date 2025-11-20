using Gestor_Gastos.Models;

namespace Gestor_Gastos.Models.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int TotalUsuarios { get; set; }
        public int UsuariosActivos { get; set; }
        public int TotalGastos { get; set; }
        public decimal TotalMontoGastos { get; set; }
        public List<GastoPorCategoria> GastosPorCategoria { get; set; } = new List<GastoPorCategoria>();
        public List<GastoPorMes> GastosPorMes { get; set; } = new List<GastoPorMes>();
        public List<AdminUsuarioResumen> UsuariosResumen { get; set; } = new List<AdminUsuarioResumen>();
    }

    public class UsuarioDetalleViewModel
    {
        public Usuario Usuario { get; set; } = null!;
        public List<Gasto> Gastos { get; set; } = new List<Gasto>();
        public int TotalGastos { get; set; }
        public decimal TotalMonto { get; set; }
        public decimal PromedioGasto { get; set; }
        public List<GastoPorCategoria> GastosPorCategoria { get; set; } = new List<GastoPorCategoria>();
    }

    public class AdminUsuarioResumen
    {
        public int UsuarioId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public bool Activo { get; set; }
        public int CantidadGastos { get; set; }
        public decimal TotalMonto { get; set; }
        public decimal PromedioGasto { get; set; }
    }
}
