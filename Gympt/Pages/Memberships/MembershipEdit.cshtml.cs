using Gympt.DTO;
using Gympt.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gympt.Pages.Memberships
{
    public class MembershipEditModel : PageModel
    {
        private readonly MembershipApiClient _membershipApiClient;

        [BindProperty]
        public MembershipDTO Membership { get; set; } = new();

        public MembershipEditModel(MembershipApiClient membershipApiClient)
        {
            _membershipApiClient = membershipApiClient;
        }

        // Se ejecuta para cargar los datos en el formulario
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Membership = await _membershipApiClient.GetMembershipByIdAsync(id);

            if (Membership == null)
            {
                return NotFound(); // Si la API no encuentra la membresía, muestra un error 404
            }
            return Page();
        }

        // Se ejecuta al enviar el formulario con los cambios
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await _membershipApiClient.UpdateMembershipAsync(Membership.Id, Membership);
                return RedirectToPage("./Memberships");
            }
            catch (ApiException ex)
            {
                ModelState.AddModelError(string.Empty, $"Error desde la API: {ex.ApiErrorContent}");
                return Page();
            }
        }
    }
}