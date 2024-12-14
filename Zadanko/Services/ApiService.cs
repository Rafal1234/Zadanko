using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Zadanko.Models;

namespace Zadanko.Services
{
    public interface IApiService
    {
        Task<int> AddNumbersAsync(int firstNumber, int secondNumber);
    }

    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new System.Uri("https://localhost:5001/");
        }

        public async Task<int> AddNumbersAsync(int firstNumber, int secondNumber)
        {
            var request = new AddRequest(firstNumber, secondNumber);

            var response = await _httpClient.PostAsJsonAsync("add", request);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<AddResponse>();
            return result?.Result ?? 0;
        }
    }
}
