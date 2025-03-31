using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Sample.Client.Providers;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Sample.Client.Models;

namespace Sample.Client.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    private readonly AuthenticationStateProvider _authStateProvider;
    private const string TOKEN_KEY = "authToken";

    public AuthService(HttpClient httpClient, ILocalStorageService localStorage, AuthenticationStateProvider authStateProvider)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
        _authStateProvider = authStateProvider;
    }

    public async Task<bool> LoginAsync(string username, string password)
    {
        var loginModel = new LoginModel
        {
            Username = username,
            Password = password
        };

        var response = await _httpClient.PostAsJsonAsync("api/Auth/login", loginModel);
        
        if (!response.IsSuccessStatusCode)
            return false;
            
        var content = await response.Content.ReadFromJsonAsync<AuthResponseModel>();
        if (content?.token == null)
            return false;
            
        await _localStorage.SetItemAsync(TOKEN_KEY, content.token);
        
        ((CustomAuthStateProvider)_authStateProvider).NotifyUserAuthentication(content.token);
        
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", content.token);
        
        return true;
    }

    public async Task<bool> RegisterAsync(string username, string email, string password)
    {
        var registerModel = new RegisterModel
        {
            Username = username,
            Email = email,
            Password = password
        };

        var response = await _httpClient.PostAsJsonAsync("api/Auth/register", registerModel);
        return response.IsSuccessStatusCode;
    }

    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync(TOKEN_KEY);
        
        _httpClient.DefaultRequestHeaders.Authorization = null;
        
        ((CustomAuthStateProvider)_authStateProvider).NotifyUserLogout();
    }

    public async Task<bool> IsUserAuthenticatedAsync()
    {
        var token = await _localStorage.GetItemAsync<string>(TOKEN_KEY);
        return !string.IsNullOrEmpty(token);
    }
}