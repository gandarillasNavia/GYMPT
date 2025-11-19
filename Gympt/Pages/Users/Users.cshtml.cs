using Microsoft.AspNetCore.Mvc;
using Gympt.DTO;
using Gympt.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GYMPT.Pages.Users
{
    public class UsersModel : PageModel
    {
        private readonly UserApiClient _userApiClient;
        public IEnumerable<UserDTO> UserList { get; private set; }

        public UsersModel(UserApiClient userApiClient)
        {
            _userApiClient = userApiClient;
        }

        public async Task OnGetAsync()
        {
            UserList = await _userApiClient.GetAllUsersAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _userApiClient.DeleteUserAsync(id);

            return RedirectToPage();
        }
    }
}