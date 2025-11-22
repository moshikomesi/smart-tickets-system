using SupportTickets.Api.Domain;

namespace SupportTickets.Api.DTOs;

public class TicketResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
    public string? Summary { get; set; }

    public string? ImageUrl { get; set; }

    public string Status { get; set; } = "New";
    public string? Resolution { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
