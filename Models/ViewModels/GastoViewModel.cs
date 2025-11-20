using System.ComponentModel.DataAnnotations;

namespace Gestor_Gastos.Models.ViewModels
{
    public class GastoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La categoría es obligatoria")]
        [Display(Name = "Categoría")]
        public int CategoriaId { get; set; }

        [Required(ErrorMessage = "El monto es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0")]
        [Display(Name = "Monto ($ COP)")]
        public decimal Monto { get; set; }

        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "La fecha del gasto es obligatoria")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha del gasto")]
        public DateTime FechaGasto { get; set; } = DateTime.Today;

        // Para mostrar en vistas
        public string? CategoriaNombre { get; set; }
        public string? CategoriaColor { get; set; }
    }
}
