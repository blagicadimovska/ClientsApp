using ClientsApplication.Models;
using Microsoft.Extensions.Logging;
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

        private readonly ILogger<ClientsApiService> _logger;


        public ClientsApiService(HttpClient httpClient, ILogger<ClientsApiService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<Client>> SearchClientsAsync(string name)
        {
            _logger.LogInformation("Calling web api Search clients - GET:" + _httpClient.BaseAddress + "/api/clients");

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
            _logger.LogInformation("Calling web api Get Client by Id - GET:" + _httpClient.BaseAddress + "/api/clients");

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
            _logger.LogInformation("Calling web api Update Client - PUT:" + _httpClient.BaseAddress + "/api/clients");

            var jsonContent = new StringContent(JsonConvert.SerializeObject(client), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"api/clients/{id}", jsonContent);
            return response.IsSuccessStatusCode;
        }
    }
}
