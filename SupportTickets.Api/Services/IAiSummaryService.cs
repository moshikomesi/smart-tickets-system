namespace SupportTickets.Api.Services;

public interface IAiSummaryService
{
    Task<string?> GenerateSummaryAsync(string description);
}