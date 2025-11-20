// Ruta: Gympt/Pages/Users/UserCreateModel.cs
using Gympt.DTO;
using Gympt.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text.Json; // Necesario para leer el JSON
using System.Threading.Tasks;

namespace Gympt.Pages.Users
{
    public class UserCreateModel : PageModel
    {
        private readonly UserApiClient _userApiClient;

        [BindProperty]
        public UserDTO User { get;set; } = new UserDTO();

        public UserCreateModel(UserApiClient userApiClient)
        {
            _userApiClient = userApiClient;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            // ... (Toda tu l�gica de validaci�n manual se queda igual) ...
            if (User.Role == "Instructor")
            {
                if (!User.HireDate.HasValue)
                    ModelState.AddModelError("User.HireDate", "La fecha de contrataci�n es requerida para un instructor.");
                if (!User.MonthlySalary.HasValue || User.MonthlySalary <= 0)
                    ModelState.AddModelError("User.MonthlySalary", "El salario debe ser mayor a cero para un instructor.");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                User.Password = "gympt" + User.Ci;
                await _userApiClient.CreateUserAsync(User);
            }
            catch (ApiException ex) // <-- AHORA ATRAPAMOS NUESTRA EXCEPCI�N PERSONALIZADA
            {
                // El mensaje de la excepci�n AHORA S� es el mensaje real de la API.
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
            catch (HttpRequestException ex) // Mantenemos un catch para errores de red
            {
                ModelState.AddModelError(string.Empty, $"Error de conexi�n: {ex.Message}");
                return Page();
            }

            TempData["SuccessMessage"] = $"Usuario '{User.Name} {User.FirstLastname}' creado exitosamente.";
            return RedirectToPage("/Users/Users");
        }
    }
}