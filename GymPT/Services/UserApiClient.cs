// Ruta: Gympt/Services/UserApiClient.cs
using Gympt.DTO;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Gympt.Services
{
    public class UserApiClient
    {
        private readonly HttpClient _httpClient;

        public UserApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // --- MÉTODOS CRUD ACTUALIZADOS ---

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<UserDTO>>("api/users");
        }

        public async Task<UserDTO> CreateUserAsync(UserDTO newUser)
        {
            var response = await _httpClient.PostAsJsonAsync("api/users", newUser);
            await HandleApiResponse(response); // Llama a nuestro manejador de errores
            return await response.Content.ReadFromJsonAsync<UserDTO>();
        }

        public async Task UpdateUserAsync(int id, UserDTO userToUpdate)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/users/{id}", userToUpdate);
            await HandleApiResponse(response); // Llama a nuestro manejador de errores
        }

        public async Task DeleteUserAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/users/{id}");
            await HandleApiResponse(response); // Llama a nuestro manejador de errores
        }

        // --- EL NUEVO MÉTODO INTELIGENTE ---
        private async Task HandleApiResponse(HttpResponseMessage response)
        {
            // Si la respuesta es exitosa (código 2xx), no hacemos nada.
            if (response.IsSuccessStatusCode)
            {
                return;
            }

            // Si no es exitosa, leemos el mensaje de error del cuerpo.
            string errorContent = await response.Content.ReadAsStringAsync();
            string errorMessage = "La API devolvió un error, pero no se pudo leer el mensaje.";

            try
            {
                // Intentamos parsear el JSON para encontrar la propiedad "error"
                using (JsonDocument doc = JsonDocument.Parse(errorContent))
                {
                    if (doc.RootElement.TryGetProperty("error", out JsonElement errorElement))
                    {
                        errorMessage = errorElement.GetString();
                    }
                }
            }
            catch (JsonException)
            {
                // Si el cuerpo no es JSON, usamos el contenido directamente (si no está vacío).
                if (!string.IsNullOrEmpty(errorContent))
                {
                    errorMessage = errorContent;
                }
            }

            // Lanzamos nuestra excepción personalizada con el mensaje de error real.
            throw new ApiException(errorMessage, response.StatusCode);
        }
    }
}