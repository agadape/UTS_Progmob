using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;


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

        // GET by id
        public async Task<T?> GetByIdAsync<T>(string url)
        {
            return await _http.GetFromJsonAsync<T>(url);
        }


        public async Task<bool> UpdateAsync<T>(string url, int id, T data)
        {
            var response = await _http.PutAsJsonAsync($"{url}/{id}", data);
            return response.IsSuccessStatusCode;
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
        public async Task<string?> UploadImageAsync(IBrowserFile file)
        {
            var content = new MultipartFormDataContent();
            var stream = file.OpenReadStream(5 * 1024 * 1024);

            var fileContent = new StreamContent(stream);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);

            content.Add(fileContent, "file", file.Name);

            var response = await _http.PostAsync("api/upload/image", content);
            if (!response.IsSuccessStatusCode)
                return null;

            var result = await response.Content.ReadFromJsonAsync<UploadResult>();
            return result?.Url;
        }

        private class UploadResult
        {
            public string? Url { get; set; }
        }


    }
}
