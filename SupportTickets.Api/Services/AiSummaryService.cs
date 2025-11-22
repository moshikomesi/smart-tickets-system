using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace SupportTickets.Api.Services;

public class AiSummaryService : IAiSummaryService
{
    private readonly string _apiKey;
    private readonly HttpClient _http;

    public AiSummaryService(IConfiguration config)
    {
        _apiKey = config["AI:ApiKey"] ?? throw new Exception("Missing Gemini API key");
        _http = new HttpClient();
    }

    public async Task<string> GenerateSummaryAsync(string description)
    {
        var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={_apiKey}";

        var requestBody = new
        {
            contents = new[]
            {
                new {
                    parts = new[]
                    {
                        new { text = $"Summarize this customer support issue in 1–2 short sentences:\n\n{description}" }
                    }
                }
            }
        };

        var json = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _http.PostAsync(url, content);
        var responseJson = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(responseJson);

        try
        {
            return doc.RootElement
                     .GetProperty("candidates")[0]
                     .GetProperty("content")
                     .GetProperty("parts")[0]
                     .GetProperty("text")
                     .GetString()
                   ?? "Summary unavailable";
        }
        catch
        {
            return "Summary unavailable";
        }
    }
}
