using System.ComponentModel.DataAnnotations;

namespace Gympt.DTO
{
    public class DisciplineDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required(ErrorMessage = "La hora de inicio es obligatoria.")]
        public TimeSpan StartTime { get; set; }

        [Required(ErrorMessage = "La hora de fin es obligatoria.")]
        public TimeSpan EndTime { get; set; }

        [Display(Name = "ID del Instructor")]
        public int? InstructorId { get; set; }
    }
}