 // Asegúrate de que tu ClientDTO esté en este namespace
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Gympt.DTO;

namespace Gympt.Services
{
    public class ClientApiService
    {
        private readonly HttpClient _http;

        public ClientApiService(HttpClient http)
        {
            _http = http;
        }

        // Obtener todos los clientes
        public async Task<List<ClientDTO>> GetClientsAsync()
        {
            // Cambié "api/client" a "api/clients" para que coincida con el microservicio
            return await _http.GetFromJsonAsync<List<ClientDTO>>("api/clients");
        }

        // Obtener cliente por ID
        public async Task<ClientDTO> GetClientByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<ClientDTO>($"api/clients/{id}");
        }

        // Crear nuevo cliente
        public async Task<ClientDTO> CreateClientAsync(ClientDTO client)
        {
            var response = await _http.PostAsJsonAsync("api/clients", client);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ClientDTO>();
        }

        // Actualizar cliente existente
        public async Task<bool> UpdateClientAsync(int id, ClientDTO client)
        {
            var response = await _http.PutAsJsonAsync($"api/clients/{id}", client);
            return response.IsSuccessStatusCode;
        }

        // Eliminar cliente
        public async Task<bool> DeleteClientAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/clients/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
