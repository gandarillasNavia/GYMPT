using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Gympt.DTO;
using Gympt.Common;

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
            return await _http.GetFromJsonAsync<List<ClientDTO>>("api/clients");
        }

        // Obtener cliente por ID
        public async Task<ClientDTO> GetClientByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<ClientDTO>($"api/clients/{id}");
        }

        // Crear nuevo cliente
        public async Task<Result<ClientDTO>> CreateClientAsync(ClientDTO client)
        {
            // Asigna valores de negocio por defecto
            client.CreatedAt = DateTime.UtcNow;
            client.CreatedBy = "System";
            
            // Aquí verificamos el objeto antes de enviarlo
            Console.WriteLine("Enviando cliente al microservicio:");
            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(client));

            var response = await _http.PostAsJsonAsync("api/clients", client);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Error recibido del microservicio: " + errorMessage);
                return Result<ClientDTO>.Failure(errorMessage);
            }


            var createdClient = await response.Content.ReadFromJsonAsync<ClientDTO>();
            Console.WriteLine("Cliente creado correctamente: " + System.Text.Json.JsonSerializer.Serialize(createdClient));
            return Result<ClientDTO>.Success(createdClient);
        }


        // Actualizar cliente existente
        public async Task<Result<ClientDTO>> UpdateClientAsync(int id, ClientDTO client)
        {
            // Si no se especifica quién modifica, se asigna "System" por defecto
            client.LastModification = DateTime.UtcNow;
            client.LastModifiedBy ??= "System";

            var response = await _http.PutAsJsonAsync($"api/clients/{id}", client);

            if (!response.IsSuccessStatusCode)
            {
                var errorObj = await response.Content.ReadFromJsonAsync<ErrorDTO>();
                return Result<ClientDTO>.Failure(errorObj?.Error ?? "Error desconocido al crear el cliente.");
            }

            var updatedClient = await response.Content.ReadFromJsonAsync<ClientDTO>();
            return Result<ClientDTO>.Success(updatedClient);
        }

        // Eliminar cliente
        public async Task<bool> DeleteClientAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/clients/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}