using System.ComponentModel.DataAnnotations;

namespace Gympt.DTO
{
    public class MembershipDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0.01, 10000.00, ErrorMessage = "El precio debe ser un valor positivo.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "El número de sesiones es obligatorio.")]
        [Range(1, 100, ErrorMessage = "Las sesiones deben ser entre 1 y 100.")]
        [Display(Name = "Sesiones Mensuales")]
        public short MonthlySessions { get; set; }
    }
}