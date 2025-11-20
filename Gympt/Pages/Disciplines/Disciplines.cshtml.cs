using Gympt.DTO;
using Gympt.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gympt.Pages.Disciplines
{
    public class DisciplinesModel : PageModel
    {
        private readonly DisciplineApiClient _disciplineApiClient;

        public List<DisciplineDTO> Disciplines { get; set; } = new();

        public DisciplinesModel(DisciplineApiClient disciplineApiClient)
        {
            _disciplineApiClient = disciplineApiClient;
        }

        public async Task OnGetAsync()
        {
            try
            {
                Disciplines = await _disciplineApiClient.GetDisciplinesAsync();
            }
            catch (ApiException ex)
            {
                ModelState.AddModelError(string.Empty, $"Error al cargar datos: {ex.Message}");
            }
        }
    }
}