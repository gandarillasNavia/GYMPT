using Gympt.DTO;
using Gympt.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

public class CreateClientModel : PageModel
{
    private readonly ClientApiService _api;

    public CreateClientModel(ClientApiService api)
    {
        _api = api;
    }

    [BindProperty]
    public ClientDTO Client { get; set; } = new();

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        try
        {
            // Aquí llamamos al método correcto y pasamos la propiedad Client
            var createdClient = await _api.CreateClientAsync(Client);

            if (createdClient != null)
                return RedirectToPage("/Clients/Clients");

            ModelState.AddModelError("", "No se pudo crear el cliente.");
            return Page();
        }
        catch (Exception ex)
        {
            // Opcional: agregar logging si quieres
            ModelState.AddModelError("", $"Error al crear el cliente: {ex.Message}");
            return Page();
        }
    }
}
