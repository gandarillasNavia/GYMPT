using Gympt.DTO;
using System.Text;
using System.Text.Json;

namespace Gympt.Services
{
    public class DisciplineApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public DisciplineApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        // --- MÉTODO GET (Todas las Disciplinas) --- (Sin cambios)
        public async Task<List<DisciplineDTO>> GetDisciplinesAsync()
        {
            var response = await _httpClient.GetAsync("api/Disciplines");
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new ApiException("Error al obtener las disciplinas.", (int)response.StatusCode, error);
            }
            var stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<List<DisciplineDTO>>(stream, _jsonOptions) ?? new List<DisciplineDTO>();
        }

        // --- MÉTODO POST (Crear una Disciplina) --- (Sin cambios)
        public async Task CreateDisciplineAsync(DisciplineDTO discipline)
        {
            var jsonContent = new StringContent(JsonSerializer.Serialize(discipline), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/Disciplines", jsonContent);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new ApiException("Error al crear la disciplina.", (int)response.StatusCode, error);
            }
        }

        // --- NUEVO MÉTODO GET (Una Disciplina por ID) ---
        // Este método soluciona el error CS1061 en OnGetAsync
        public async Task<DisciplineDTO?> GetDisciplineByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/Disciplines/{id}");

            // Si la API devuelve 404 Not Found, devolvemos null para que la página pueda manejarlo.
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new ApiException($"No se pudo obtener la disciplina con ID {id}.", (int)response.StatusCode, error);
            }
            var stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<DisciplineDTO>(stream, _jsonOptions);
        }

        // --- NUEVO MÉTODO PUT (Actualizar una Disciplina) ---
        // Este método soluciona el error CS1061 en OnPostAsync
        public async Task UpdateDisciplineAsync(int id, DisciplineDTO discipline)
        {
            var jsonContent = new StringContent(JsonSerializer.Serialize(discipline), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"api/Disciplines/{id}", jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new ApiException("Error al actualizar la disciplina.", (int)response.StatusCode, error);
            }
        }
    }
}