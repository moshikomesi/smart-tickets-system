namespace SupportTickets.Api.Services;

public class ConsoleEmailService : IEmailService
{
    public Task SendTicketCreatedAsync(string to, Guid ticketId)
    {
        Console.WriteLine($"[EMAIL] To: {to} | Ticket created: {ticketId}");
        return Task.CompletedTask;
    }

    public Task SendStatusChangedAsync(string to, Guid ticketId, string newStatus)
    {
        Console.WriteLine($"[EMAIL] To: {to} | Ticket {ticketId} status changed to {newStatus}");
        return Task.CompletedTask;
    }

    public Task SendResolutionChangedAsync(string to, Guid ticketId)
    {
        Console.WriteLine($"[EMAIL] To: {to} | Ticket {ticketId} resolution updated");
        return Task.CompletedTask;
    }
}
