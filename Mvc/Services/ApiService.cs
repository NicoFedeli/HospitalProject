using System.Text;
using Hospital.Models;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;

public interface IApiService
{
    Task<ApiResponse<T>> GetAsync<T>(string relativeUrl, Dictionary<string, string> queryParams); // Passo come parametro l'URL relativo, seguito da eventuali query string
    Task<ApiResponse<T>> PostAsync<T>(string relativeUrl, object? data);
    Task<ApiResponse<T>> PutAsync<T>(string relativeUrl, object? data);
    Task<ApiResponse<T>> DeleteAsync<T>(string relativeUrl);
}

public class ApiService : IApiService
{
    private readonly HttpClient _httpClient;
    public ApiService(HttpClient httpClient) => _httpClient = httpClient;

    // Implementazione dei metodi dell'interfaccia IApiService
    // Evito così di ripetere la chiamta HttpClient in ogni controller

    public async Task<ApiResponse<T>> GetAsync<T>(string url, Dictionary<string, string> queryParams)
    {
        var finalUrl = url;
        if (queryParams != null && queryParams.Count != 0)
        {
            var query = string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}"));
            finalUrl = $"{url}?{query}";
        }
        var response = await _httpClient.GetAsync(finalUrl);
        var content = await response.Content.ReadAsStringAsync();
        return DeserializeApiResponse<T>(content, response.IsSuccessStatusCode);
    }

    public async Task<ApiResponse<T>> PostAsync<T>(string relativeUrl, object? data)
    {
        var payload = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
        var resp = await _httpClient.PostAsync(relativeUrl, payload);
        var content = await resp.Content.ReadAsStringAsync();
        return DeserializeApiResponse<T>(content, resp.IsSuccessStatusCode);
    }

    public async Task<ApiResponse<T>> PutAsync<T>(string relativeUrl, object? data)
    {
        var payload = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
        var resp = await _httpClient.PutAsync(relativeUrl, payload);
        var content = await resp.Content.ReadAsStringAsync();
        return DeserializeApiResponse<T>(content, resp.IsSuccessStatusCode);
    }

    public async Task<ApiResponse<T>> DeleteAsync<T>(string relativeUrl)
    {
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
