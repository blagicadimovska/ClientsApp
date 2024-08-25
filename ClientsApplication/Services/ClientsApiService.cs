using ClientsApplication.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ClientsApplication.Services
{
    public class ClientsApiService
    {
        private readonly HttpClient _httpClient;

        public ClientsApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Client>> SearchClientsAsync(string name)
        {
            var response = await _httpClient.GetAsync($"api/clients?name={Uri.EscapeDataString(name)}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Client>>(json);
            }
            return new List<Client>(); 
        }

        public async Task<Client> GetClientByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/clients/{id}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Client>(json);
            }
            return null;
        }

        public async Task<bool> UpdateClientAsync(int id, Client client)
        {
            var jsonContent = new StringContent(JsonConvert.SerializeObject(client), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"api/clients/{id}", jsonContent);
            return response.IsSuccessStatusCode;
        }
    }
}
