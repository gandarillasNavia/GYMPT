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

        // Se ejecuta cuando se carga la p�gina (GET).
        // Su trabajo es obtener los datos del instructor y rellenar el formulario.
        public async Task<IActionResult> OnGetAsync(int id)
        {
            //Instructor = await _userApiClient.GetUserByIdAsync(id);

            // Verificaci�n de seguridad importante:
            // 1. �Existe el usuario?
            // 2. �Es realmente un instructor?
            if (Instructor == null || Instructor.Role != "Instructor")
            {
                // Si no se encuentra o alguien intenta editar un Admin por la URL,
                // lo enviamos a una p�gina de error.
                return NotFound();
            }

            return Page();
        }

        // Se ejecuta cuando se env�a el formulario (POST).
        // Su trabajo es validar los datos y enviarlos a la API.
        public async Task<IActionResult> OnPostAsync()
        {
            // Verifica si los datos del formulario cumplen las reglas de validaci�n del DTO.
            if (!ModelState.IsValid)
            {
                // Si no son v�lidos, se vuelve a mostrar la p�gina con los mensajes de error.
                return Page();
            }

            try
            {
                // Llama al ApiClient para enviar los datos actualizados.
                await _userApiClient.UpdateUserAsync(Instructor.Id, Instructor);
            }
            catch (HttpRequestException ex)
            {
                // Si la API devuelve un error (ej. validaci�n del lado del servidor),
                // lo mostramos como un error general en el formulario.
                ModelState.AddModelError(string.Empty, $"Ocurri� un error al contactar la API: {ex.Message}");
                return Page();
            }

            // Si la operaci�n es exitosa, redirigimos al usuario a la lista principal.
            // Esto sigue el patr�n Post-Redirect-Get, una pr�ctica recomendada.
            return RedirectToPage("/Users/Users");
        }
    }
}