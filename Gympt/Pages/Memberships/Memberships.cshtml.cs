using Gympt.DTO;
using Gympt.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gympt.Pages.Memberships
{
    public class MembershipsModel : PageModel
    {
        private readonly MembershipApiClient _membershipApiClient;

        // Propiedad para almacenar la lista de membresías que se mostrará en la página
        public IList<MembershipDTO> Memberships { get; set; } = new List<MembershipDTO>();

        public MembershipsModel(MembershipApiClient membershipApiClient)
        {
            _membershipApiClient = membershipApiClient;
        }

        // Se ejecuta cuando se carga la página
        public async Task OnGetAsync()
        {
            // Llama a la API para obtener la lista de todas las membresías
            Memberships = await _membershipApiClient.GetMembershipsAsync();
        }
    }
}

