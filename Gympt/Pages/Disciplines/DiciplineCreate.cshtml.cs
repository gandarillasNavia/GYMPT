using Gympt.DTO;
using Gympt.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gympt.Pages.Disciplines
{
    public class DisciplineCreateModel : PageModel
    {
        private readonly DisciplineApiClient _disciplineApiClient;

        [BindProperty]
        public DisciplineDTO Discipline { get; set; } = new();

        public DisciplineCreateModel(DisciplineApiClient disciplineApiClient)
        {
            _disciplineApiClient = disciplineApiClient;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // <-- PUNTO DE INTERRUPCIÓN 1 AQUÍ
            // (Haz clic en el margen gris de la izquierda de la siguiente línea)
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await _disciplineApiClient.CreateDisciplineAsync(Discipline);
                return RedirectToPage("./Disciplines");
            }
            catch (ApiException ex)
            {
                ModelState.AddModelError(string.Empty, $"Error desde la API: {ex.ApiErrorContent}");
                return Page();
            }
        }
    }
}