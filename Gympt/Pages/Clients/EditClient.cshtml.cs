using Gympt.DTO;
using Gympt.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class EditClientModel : PageModel
{
    private readonly ClientApiService _api;

    public EditClientModel(ClientApiService api)
    {
        _api = api;
    }

    [BindProperty]
    public ClientDTO Client { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Client = await _api.GetClientByIdAsync(id);

        if (Client == null)
            return RedirectToPage("/Clients/Clients");

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        bool ok = await _api.UpdateClientAsync(Client.Id, Client);


        if (ok)
            return RedirectToPage("/Clients/Clients");

        ModelState.AddModelError("", "No se pudo actualizar el cliente.");
        return Page();
    }
}
