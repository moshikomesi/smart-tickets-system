using SupportTickets.Api.Domain;

namespace SupportTickets.Api.DTOs;

public class UpdateTicketRequest
{
    public string? Status { get; set; }
    public string? Resolution { get; set; }
}
