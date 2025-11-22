namespace SupportTickets.Api.Services;

public class DummyAiSummaryService : IAiSummaryService
{
    public Task<string?> GenerateSummaryAsync(string description)
    {
        // OpenAI/Gemini
        var shortSummary = description.Length > 80
            ? description.Substring(0, 80) + "..."
            : description;

        return Task.FromResult<string?>(shortSummary);
    }
}
