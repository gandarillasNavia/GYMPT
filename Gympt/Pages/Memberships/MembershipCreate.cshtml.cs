using Gympt.DTO;
using Gympt.Services; // <-- ESTA LÍNEA ES CRUCIAL. Soluciona el error.
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gympt.Pages.Memberships
{
    public class MembershipCreateModel : PageModel
    {
        // El compilador ahora sabe qué es "MembershipApiClient" gracias a la directiva 'using' de arriba.
        private readonly MembershipApiClient _membershipApiClient;

        [BindProperty]
        public MembershipDTO Membership { get; set; } = new();

        public MembershipCreateModel(MembershipApiClient membershipApiClient)
        {
            _membershipApiClient = membershipApiClient;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                await _membershipApiClient.CreateMembershipAsync(Membership);
                return RedirectToPage("./Memberships"); // Asume que tienes una página de lista
            }
            catch (ApiException ex)
            {
                ModelState.AddModelError(string.Empty, $"Error desde la API: {ex.ApiErrorContent}");
                return Page();
            }
        }
    }
}