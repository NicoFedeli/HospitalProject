using System.Net.Http.Headers;
using System.Text;
using Hospital.Models;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;

public interface IApiHelper
{
    Task<ApiResponse<T>> GetAsync<T>(string relativeUrl, object? queryParams = null); // Passo come parametro l'URL relativo, seguito da eventuali query parameters
    Task<ApiResponse<T>> PostAsync<T>(string relativeUrl, object? data=null);
    Task<ApiResponse<T>> PutAsync<T>(string relativeUrl, object? data=null);
    Task<ApiResponse<T>> DeleteAsync<T>(string relativeUrl);
}

public class ApiHelper : IApiHelper
{
    private readonly HttpClient _httpClient; // Iniettato tramite DI(Dependency Injection) in Program.cs
    private readonly IHttpContextAccessor _httpContextAccessor; // Per accedere alla sessione
    public ApiHelper(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
    }

    // Implementazione dei metodi dell'interfaccia IApiHelper
    // Evito così di ripetere la chiamta HttpClient in ogni controller

    // Aggiungo l'header di autorizzazione con il token JWT
    private void AddAuthorizationHeader()
    {
        var token = _httpContextAccessor.HttpContext?.User?.FindFirst("Token")?.Value;
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }

    // Converte un oggetto anonimo o un dizionario in query string
    private string ToQueryString(object obj)
    {
        if (obj is Dictionary<string, string> dict)
        {
            return string.Join("&", dict.Select(kv => $"{kv.Key}={Uri.EscapeDataString(kv.Value)}"));
        }

        // Converti oggetto anonimo in query string
        var props = obj.GetType().GetProperties();
        return string.Join("&", props.Select(p => $"{p.Name}={Uri.EscapeDataString(p.GetValue(obj)?.ToString() ?? string.Empty)}"));
    }

    public async Task<ApiResponse<T>> GetAsync<T>(string url, object? queryParams = null)
    {
        if (queryParams != null)
        {
            var query = ToQueryString(queryParams);
            url = $"{url}?{query}";
        }

        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"DEBUG Raw content from API: {content}");


        // Se non OK, logga header WWW-Authenticate per capire perché
        if (!response.IsSuccessStatusCode)
        {
            var www = response.Headers.WwwAuthenticate.FirstOrDefault()?.ToString() ?? "";
            Console.WriteLine($"API GET {url} returned {(int)response.StatusCode} {response.ReasonPhrase}. WWW-Authenticate: {www}");
        }

        return DeserializeApiResponse<T>(content, response.IsSuccessStatusCode);
    }

    public async Task<ApiResponse<T>> PostAsync<T>(string relativeUrl, object? data)
    {
        AddAuthorizationHeader(); // Aggiungo l'header di autorizzazione
        var payload = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
        var resp = await _httpClient.PostAsync(relativeUrl, payload);
        var content = await resp.Content.ReadAsStringAsync();
        return DeserializeApiResponse<T>(content, resp.IsSuccessStatusCode);
    }

    public async Task<ApiResponse<T>> PutAsync<T>(string relativeUrl, object? data)
    {
        AddAuthorizationHeader(); // Aggiungo l'header di autorizzazione
        var payload = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
        var resp = await _httpClient.PutAsync(relativeUrl, payload);
        var content = await resp.Content.ReadAsStringAsync();
        return DeserializeApiResponse<T>(content, resp.IsSuccessStatusCode);
    }

    public async Task<ApiResponse<T>> DeleteAsync<T>(string relativeUrl)
    {
        AddAuthorizationHeader(); // Aggiungo l'header di autorizzazione
        var resp = await _httpClient.DeleteAsync(relativeUrl);
        var content = await resp.Content.ReadAsStringAsync();
        return DeserializeApiResponse<T>(content, resp.IsSuccessStatusCode);
    }

    private static ApiResponse<T> DeserializeApiResponse<T>(string content, bool ok)
    {
        // L’API restituisce { Status, Message?, Data? }
        try
        {
            var parsed = JsonConvert.DeserializeObject<ApiResponse<T>>(content);
            if (parsed != null)
                return parsed;
        }
        catch (Exception ex)
        {
            return new ApiResponse<T>
            {
                Status = "KO",
                Message = $"Errore durante la chiamata API: {ex.Message}",
                Data = default
            };
        }

        // Fallback in caso di risposte non conformi o errore
        return new ApiResponse<T>
        {
            Status = ok ? "OK" : "KO",
            Message = ok ? null : (string.IsNullOrWhiteSpace(content) ? "Errore sconosciuto" : content),
            Data = default
        };
    }
}
