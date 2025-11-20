using Gympt.DTO;
using System.Text;
using System.Text.Json;

namespace Gympt.Services
{
    // Clases para el login, si las necesitas en este archivo
    public class LoginRequest { public string Username { get; set; } = string.Empty; public string Password { get; set; } = string.Empty; }
    public class LoginResponse { public string Token { get; set; } = string.Empty; }

    public class UserApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public UserApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        // --- MÉTODO DE LOGIN (si lo tienes) ---
        public async Task<LoginResponse?> LoginAsync(LoginRequest loginRequest)
        {
            var jsonContent = new StringContent(JsonSerializer.Serialize(loginRequest), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/Auth/login", jsonContent); // Asumiendo un endpoint de login

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new ApiException("Error durante el inicio de sesión.", (int)response.StatusCode, errorContent);
            }

            var stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<LoginResponse>(stream, _jsonOptions);
        }

        // --- MÉTODO GET (Todos) --- (Soluciona el error de Users.cshtml.cs)
        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            var response = await _httpClient.GetAsync("api/Users"); // Asumiendo que el endpoint es /api/Users
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new ApiException("No se pudieron obtener los usuarios.", (int)response.StatusCode, errorContent);
            }
            var stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<List<UserDTO>>(stream, _jsonOptions) ?? new List<UserDTO>();
        }

        // --- MÉTODO GET (Por ID) --- (Necesario para la página de edición)
        public async Task<UserDTO?> GetUserByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/Users/{id}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return null;
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new ApiException("Error al obtener el usuario.", (int)response.StatusCode, errorContent);
            }
            var stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<UserDTO>(stream, _jsonOptions);
        }

        // --- MÉTODO POST (Crear) --- (Soluciona el error de UserCreate.cshtml.cs)
        public async Task<UserDTO?> CreateUserAsync(UserDTO user)
        {
            var jsonContent = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/Users", jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new ApiException("Error al crear el usuario.", (int)response.StatusCode, errorContent);
            }

            var stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<UserDTO>(stream, _jsonOptions);
        }

        // --- MÉTODO PUT (Actualizar) --- (Soluciona el error de UserEdit.cshtml.cs)
        public async Task UpdateUserAsync(int id, UserDTO user)
        {
            var jsonContent = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"api/Users/{id}", jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new ApiException("Error al actualizar el usuario.", (int)response.StatusCode, errorContent);
            }
        }

        // --- MÉTODO DELETE (Eliminar) --- (Soluciona el error de Users.cshtml.cs)
        public async Task DeleteUserAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Users/{id}");

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new ApiException("Error al eliminar el usuario.", (int)response.StatusCode, errorContent);
            }
        }
    }
}