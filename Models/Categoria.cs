using System.ComponentModel.DataAnnotations;

namespace Gestor_Gastos.Models
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(255)]
        public string? Descripcion { get; set; }

        [StringLength(7)]
        public string? Color { get; set; }

        public bool Activo { get; set; } = true;

        // Navegación
        public virtual ICollection<Gasto> Gastos { get; set; } = new List<Gasto>();
    }
}

