using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Gympt.DTO;
using Gympt.Common;

namespace Gympt.Services
{
    public class MembershipApiService
    {
        private readonly HttpClient _http;

        public MembershipApiService(HttpClient http)
        {
            _http = http;
        }

        // =============================
        // GET: Obtener todas las membresías
        // =============================
        public async Task<List<MembershipDTO>> GetMembershipsAsync()
        {
            return await _http.GetFromJsonAsync<List<MembershipDTO>>("api/memberships");
        }

        // =============================
        // GET: Obtener membresía por ID
        // =============================
        public async Task<MembershipDTO> GetMembershipByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<MembershipDTO>($"api/memberships/{id}");
        }

        // =============================
        // POST: Crear membresía
        // =============================
        public async Task<Result<MembershipDTO>> CreateMembershipAsync(MembershipDTO membership)
        {
            membership.CreatedAt = DateTime.UtcNow;
            membership.CreatedBy = "System";

            var response = await _http.PostAsJsonAsync("api/memberships", membership);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return Result<MembershipDTO>.Failure(error);
            }

            var created = await response.Content.ReadFromJsonAsync<MembershipDTO>();
            return Result<MembershipDTO>.Success(created);
        }

        // =============================
        // PUT: Actualizar membresía
        // =============================
        public async Task<Result<MembershipDTO>> UpdateMembershipAsync(int id, MembershipDTO membership)
        {
            membership.LastModification = DateTime.UtcNow;
            membership.LastModifiedBy ??= "System";

            var response = await _http.PutAsJsonAsync($"api/memberships/{id}", membership);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return Result<MembershipDTO>.Failure(error);
            }

            // Caso donde el microservicio responde 204 o sin JSON
            if (response.StatusCode == System.Net.HttpStatusCode.NoContent ||
                response.Content.Headers.ContentLength == 0)
            {
                return Result<MembershipDTO>.Success(membership);
            }

            // Intentar leer JSON si existe
            try
            {
                var updated = await response.Content.ReadFromJsonAsync<MembershipDTO>();
                return Result<MembershipDTO>.Success(updated ?? membership);
            }
            catch
            {
                // Si no se puede leer JSON, devolvemos el modelo enviado
                return Result<MembershipDTO>.Success(membership);
            }
        }


        // =============================
        // DELETE: Eliminar membresía
        // =============================
        public async Task<bool> DeleteMembershipAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/memberships/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
