using Gympt.DTO;
using Gympt.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GYMPT.Pages.Users
{
    public class UserDetailModel : PageModel
    {
        private readonly UserApiClient _userApiClient;

        public UserDTO User { get; private set; }

        public UserDetailModel(UserApiClient userApiClient)
        {
            _userApiClient = userApiClient;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            //User = await _userApiClient.GetUserByIdAsync(id);

            if (User == null)
            {
                // Si la API no devuelve un usuario, redirigimos a una página de "No Encontrado"
                return NotFound();
            }

            return Page();
        }
    }
}