using Gympt.DTO;
using System.Text.Json;
using System.Net.Http.Json;
namespace Gympt.Services
{
    public class UserApiClient
    {
        private readonly HttpClient _httpClient;

        public UserApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            // GetFromJsonAsync ya maneja la deserialización del JSON a tus objetos C#
            return await _httpClient.GetFromJsonAsync<IEnumerable<UserDTO>>("api/users");
        }
        public async Task<UserDTO> GetUserByIdAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<UserDTO>($"api/users/{id}");
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task<UserDTO> CreateUserAsync(UserDTO newUser)
        {
            var response = await _httpClient.PostAsJsonAsync("api/users", newUser);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<UserDTO>();
        }

        public async Task UpdateUserAsync(int id, UserDTO userToUpdate)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/users/{id}", userToUpdate);

            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteUserAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/users/{id}");

            response.EnsureSuccessStatusCode();
        }
    }
}
