using Outbox.Ui.Models;
using System.Text.Json;

namespace Outbox.Ui.Services;

public class ApiService(IHttpClientFactory httpClientFactory)
{
    public async Task CreateUserAsync(string name)
    {
        var httpClient = httpClientFactory.CreateClient("Api");

        var request = new CreateUserRequest { Name = name };

        var response = await httpClient.PostAsJsonAsync("users", request);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            string? userMessage = null;
            try
            {
                using var doc = JsonDocument.Parse(errorContent);
                if (doc.RootElement.TryGetProperty("error", out var errorProp))
                {
                    userMessage = errorProp.GetString();
                }
            }
            catch { /* Not JSON, ignore */ }
            throw new Exception(userMessage ?? errorContent);
        }
    }

    public async Task<List<OutboxMessageDto>> GetOutboxMessagesAsync()
    {
        var httpClient = httpClientFactory.CreateClient("Api");

        var response = await httpClient.GetFromJsonAsync<OutboxMessagesResponse>("outbox");

        return response?.Messages ?? [];
    }
}
