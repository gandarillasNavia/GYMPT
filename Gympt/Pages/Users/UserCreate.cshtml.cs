using Gympt.DTO;
using Gympt.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Gympt.Pages.Users
{
    public class UserCreateModel : PageModel
    {
        private readonly UserApiClient _userApiClient;

        // [BindProperty] conecta esta propiedad con el formulario de la vista.
        [BindProperty]
        public UserDTO User { get; set; } = new UserDTO(); // Inicializamos para evitar nulos

        // La contraseña se maneja por separado para mayor claridad y seguridad.
        [BindProperty]
        public string Password { get; set; }

        public UserCreateModel(UserApiClient userApiClient)
        {
            _userApiClient = userApiClient;
        }

        // El método OnGet es simple: solo muestra la página con el formulario vacío.
        public void OnGet()
        {
        }

        // Se ejecuta cuando se envía el formulario (POST).
        public async Task<IActionResult> OnPostAsync()
        {
            // Asignamos la contraseña al DTO justo antes de validar y enviar.
            // A futuro, aquí es donde se haría el hash.
            User.Password = this.Password;

            // Hacemos una validación manual básica antes de la del modelo.
            if (User.Role == "Instructor")
            {
                if (!User.HireDate.HasValue)
                    ModelState.AddModelError("User.HireDate", "La fecha de contratación es requerida para un instructor.");
                if (!User.MonthlySalary.HasValue || User.MonthlySalary <= 0)
                    ModelState.AddModelError("User.MonthlySalary", "El salario debe ser mayor a cero para un instructor.");
            }

            if (!ModelState.IsValid)
            {
                return Page(); // Vuelve a mostrar la página con los errores.
            }

            try
            {
                await _userApiClient.CreateUserAsync(User);
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError(string.Empty, $"Ocurrió un error al crear el usuario: {ex.Message}");
                return Page();
            }

            // TempData se usa para mostrar un mensaje de éxito en la página siguiente.
            TempData["SuccessMessage"] = $"Usuario '{User.Name} {User.FirstLastname}' creado exitosamente.";

            return RedirectToPage("/Users/Users");
        }
    }
}