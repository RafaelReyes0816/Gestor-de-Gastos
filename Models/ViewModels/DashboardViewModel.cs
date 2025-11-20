using Gestor_Gastos.Models.ViewModels;

namespace Gestor_Gastos.Models.ViewModels
{
    public class DashboardViewModel
    {
        // Totales por período
        public decimal TotalGastosMes { get; set; }
        public decimal TotalGastosSemana { get; set; }
        public decimal TotalGastosDia { get; set; }
        public decimal TotalGastosAnio { get; set; }

        // Estadísticas generales
        public int TotalGastosRegistrados { get; set; }
        public decimal PromedioGasto { get; set; }
        public decimal GastoMaximo { get; set; }
        public decimal GastoMinimo { get; set; }

        // Gastos por categoría
        public List<GastoPorCategoria> GastosPorCategoria { get; set; } = new List<GastoPorCategoria>();

        // Últimos gastos registrados
        public List<GastoViewModel> UltimosGastos { get; set; } = new List<GastoViewModel>();

        // Datos para gráficos
        public List<GastoPorMes> GastosPorMes { get; set; } = new List<GastoPorMes>();
    }

    public class GastoPorCategoria
    {
        public int CategoriaId { get; set; }
        public string CategoriaNombre { get; set; } = string.Empty;
        public string CategoriaColor { get; set; } = string.Empty;
        public decimal TotalMonto { get; set; }
        public int CantidadGastos { get; set; }
        public decimal Porcentaje { get; set; }
    }

    public class GastoPorMes
    {
        public int Anio { get; set; }
        public int Mes { get; set; }
        public string NombreMes { get; set; } = string.Empty;
        public decimal TotalMonto { get; set; }
        public int CantidadGastos { get; set; }
    }
}

