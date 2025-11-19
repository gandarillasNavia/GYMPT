using Gympt.DTO;
using Gympt.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace GYMPT.Pages.Users
{
    public class UserEditModel : PageModel
    {
        private readonly UserApiClient _userApiClient;

        // [BindProperty] conecta esta propiedad directamente con el formulario de la vista.
        // El nombre "Instructor" coincide con el 'asp-for="Instructor..."' de tu HTML.
        [BindProperty]
        public UserDTO Instructor { get; set; }

        public UserEditModel(UserApiClient userApiClient)
        {
            _userApiClient = userApiClient;
        }

        // Se ejecuta cuando se carga la página (GET).
        // Su trabajo es obtener los datos del instructor y rellenar el formulario.
        public async Task<IActionResult> OnGetAsync(int id)
        {
            //Instructor = await _userApiClient.GetUserByIdAsync(id);

            // Verificación de seguridad importante:
            // 1. ¿Existe el usuario?
            // 2. ¿Es realmente un instructor?
            if (Instructor == null || Instructor.Role != "Instructor")
            {
                // Si no se encuentra o alguien intenta editar un Admin por la URL,
                // lo enviamos a una página de error.
                return NotFound();
            }

            return Page();
        }

        // Se ejecuta cuando se envía el formulario (POST).
        // Su trabajo es validar los datos y enviarlos a la API.
        public async Task<IActionResult> OnPostAsync()
        {
            // Verifica si los datos del formulario cumplen las reglas de validación del DTO.
            if (!ModelState.IsValid)
            {
                // Si no son válidos, se vuelve a mostrar la página con los mensajes de error.
                return Page();
            }

            try
            {
                // Llama al ApiClient para enviar los datos actualizados.
                await _userApiClient.UpdateUserAsync(Instructor.Id, Instructor);
            }
            catch (HttpRequestException ex)
            {
                // Si la API devuelve un error (ej. validación del lado del servidor),
                // lo mostramos como un error general en el formulario.
                ModelState.AddModelError(string.Empty, $"Ocurrió un error al contactar la API: {ex.Message}");
                return Page();
            }

            // Si la operación es exitosa, redirigimos al usuario a la lista principal.
            // Esto sigue el patrón Post-Redirect-Get, una práctica recomendada.
            return RedirectToPage("/Users/Users");
        }
    }
}