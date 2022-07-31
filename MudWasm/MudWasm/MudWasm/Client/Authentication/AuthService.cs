using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using MudWasm.Shared;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace MudWasm.Client.Authentication
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ILocalStorageService _localStorage;

        private readonly string _storageKey = "session";

        public AuthService(HttpClient httpClient,
                           AuthenticationStateProvider authenticationStateProvider,
                           ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
            _localStorage = localStorage;
        }


        public async Task<UserSession> Login(LoginRequest request)
        {
            var loginAsJson = JsonSerializer.Serialize(request);
            var response = await _httpClient.PostAsync("api/User/login", new StringContent(loginAsJson, Encoding.UTF8, "application/json"));
            var session = JsonSerializer.Deserialize<UserSession>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (!response.IsSuccessStatusCode)
            {
                return session;
            }

            await _localStorage.SetItemAsync(_storageKey, session);
            var provider = _authenticationStateProvider as CustomAuthenticationStateProvider;
            provider.MarkUserAsAuthenticated(session);

            return session;
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync(_storageKey);
            var provider = _authenticationStateProvider as CustomAuthenticationStateProvider;
            provider.MarkUserAsLoggedOut();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }

    public interface IAuthService
    {
        public Task<UserSession> Login(LoginRequest request);
        public Task Logout();
    }
}
