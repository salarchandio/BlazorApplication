using System.Net.Http.Json;
using Models;

namespace BlazorApp_Frontend.Services
{
    public class LoginService
    {
        private readonly HttpClient _httpClient;

        public LoginService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> LoginAsync(LoginRequest loginRequest)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginRequest);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<TokenResponse>();
                return result?.Token;
            }
            return null; // Login failed
        }

    }
}
