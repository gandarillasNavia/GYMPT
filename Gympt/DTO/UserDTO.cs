namespace Gympt.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }

        // Campos de persona
        public string Name { get; set; }
        public string FirstLastname { get; set; }
        public string? SecondLastname { get; set; } // Nullable
        public DateTime DateBirth { get; set; }
        public string Ci { get; set; }
        public bool IsActive { get; set; }

        // Campos de usuario
        public string Role { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; } // Más adelante será PasswordHash
        public bool FisrtLogin { get; set; }

        // Campos de instructor (pueden ser nulos)
        public DateTime? HireDate { get; set; }
        public decimal? MonthlySalary { get; set; }
        public string? Specialization { get; set; }

        // Campos de auditoría
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? LastModification { get; set; }
        public string? LastModifiedBy{get; set;}
    }
}
