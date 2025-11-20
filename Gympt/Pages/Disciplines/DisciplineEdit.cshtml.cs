using Gympt.DTO;
using Gympt.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gympt.Pages.Disciplines
{
    public class DisciplineEditModel : PageModel
    {
        private readonly DisciplineApiClient _disciplineApiClient;

        [BindProperty]
        public DisciplineDTO Discipline { get; set; } = new();

        public DisciplineEditModel(DisciplineApiClient disciplineApiClient)
        {
            _disciplineApiClient = disciplineApiClient;
        }

        // --- OnGetAsync: Se ejecuta cuando la página de edición se carga ---
        public async Task<IActionResult> OnGetAsync(int id)
        {
            // Llama al nuevo método para obtener los datos de la disciplina a editar
            Discipline = await _disciplineApiClient.GetDisciplineByIdAsync(id);

            // Si la disciplina no se encuentra (la API devolvió 404), redirige a una página de error Not Found.
            if (Discipline == null)
            {
                return NotFound();
            }

            // Si se encuentra, muestra la página con los datos cargados en el formulario.
            return Page();
        }

        // --- OnPostAsync: Se ejecuta cuando se envía el formulario con los cambios ---
        public async Task<IActionResult> OnPostAsync()
        {
            // Valida que los datos del formulario sean correctos (ej. campos requeridos).
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                // Llama al nuevo método para enviar los datos actualizados a la API.
                await _disciplineApiClient.UpdateDisciplineAsync(Discipline.Id, Discipline);

                // Si todo va bien, redirige al usuario de vuelta a la lista de disciplinas.
                return RedirectToPage("./Disciplines");
            }
            catch (ApiException ex)
            {
                // Si la API devuelve un error, lo muestra en la página.
                ModelState.AddModelError(string.Empty, $"Error desde la API: {ex.ApiErrorContent}");
                return Page();
            }
        }
    }
}