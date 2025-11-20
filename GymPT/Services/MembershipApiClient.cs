using Gympt.DTO;
using System.Text;
using System.Text.Json;

namespace Gympt.Services
{
    public class MembershipApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public MembershipApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<List<MembershipDTO>> GetMembershipsAsync()
        {
            var response = await _httpClient.GetAsync("api/Memberships");
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new ApiException("Error al obtener las membresías.", (int)response.StatusCode, error);
            }
            var stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<List<MembershipDTO>>(stream, _jsonOptions) ?? new List<MembershipDTO>();
        }

        public async Task CreateMembershipAsync(MembershipDTO membership)
        {
            var jsonContent = new StringContent(JsonSerializer.Serialize(membership), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/Memberships", jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new ApiException("Error al crear la membresía.", (int)response.StatusCode, error);
            }
        }

        // --- MÉTODO NUEVO: Obtener una membresía por su ID ---
        public async Task<MembershipDTO?> GetMembershipByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/Memberships/{id}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null; // Devuelve null si la API no encuentra la membresía
            }
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new ApiException($"No se pudo obtener la membresía con ID {id}.", (int)response.StatusCode, error);
            }
            var stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<MembershipDTO>(stream, _jsonOptions);
        }

        // --- MÉTODO NUEVO: Actualizar una membresía existente ---
        public async Task UpdateMembershipAsync(int id, MembershipDTO membership)
        {
            var jsonContent = new StringContent(JsonSerializer.Serialize(membership), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"api/Memberships/{id}", jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new ApiException("Error al actualizar la membresía.", (int)response.StatusCode, error);
            }
        }
    }
}