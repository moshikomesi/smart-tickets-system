namespace SupportTickets.Api.Services;

public interface IEmailService
{
    Task SendTicketCreatedAsync(string to, Guid ticketId);
    Task SendStatusChangedAsync(string to, Guid ticketId, string newStatus);
    Task SendResolutionChangedAsync(string to, Guid ticketId);
}
