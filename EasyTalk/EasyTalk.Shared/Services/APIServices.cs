using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace EasyTalk.Shared.Services
{
    public class APIServices
    {
        private readonly HttpClient _http;
        public APIServices(HttpClient http) => _http = http;

        // Generic GET
        public async Task<List<T>> GetAllAsync<T>(string url)
        {
            return await _http.GetFromJsonAsync<List<T>>(url) ?? new();
        }

        public async Task<T?> GetByIdAsync<T>(string url)
        {
            return await _http.GetFromJsonAsync<T>(url);
        }

        public async Task<bool> SaveAsync<T>(string url, T data, bool isNew)
        {
            HttpResponseMessage response;
            if (isNew)
                response = await _http.PostAsJsonAsync(url, data);
            else
                response = await _http.PutAsJsonAsync(url, data);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(string url)
        {
            var response = await _http.DeleteAsync(url);
            return response.IsSuccessStatusCode;
        }
    }
}
