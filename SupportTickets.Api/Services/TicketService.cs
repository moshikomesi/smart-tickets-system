using SupportTickets.Api.Domain;
using SupportTickets.Api.DTOs;
using SupportTickets.Api.Repositories;

namespace SupportTickets.Api.Services;

public class TicketService : ITicketService
{
    private readonly ITicketRepository _repo;
    private readonly IEmailService _email;
    private readonly IAiSummaryService _ai;

    public TicketService(
        ITicketRepository repo,
        IEmailService email,
        IAiSummaryService ai)
    {
        _repo = repo;
        _email = email;
        _ai = ai;
    }

    public async Task<List<TicketResponse>> GetAllAsync(string? status, string? search)
    {
        var tickets = await _repo.GetAllAsync();

        if (!string.IsNullOrWhiteSpace(status))
        {
            tickets = tickets
                .Where(t => string.Equals(t.Status, status, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var lower = search.ToLower();
            tickets = tickets
                .Where(t =>
                    t.Name.ToLower().Contains(lower) ||
                    t.Description.ToLower().Contains(lower))
                .ToList();
        }

        return tickets.Select(MapToResponse).ToList();
    }

    public async Task<TicketResponse?> GetByIdAsync(Guid id)
    {
        var ticket = await _repo.GetByIdAsync(id);
        return ticket is null ? null : MapToResponse(ticket);
    }

    public async Task<TicketResponse> CreateAsync(CreateTicketRequest request)
    {
        var ticket = new Ticket
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Email = request.Email,
            Description = request.Description,
            ImageUrl = request.ImageUrl,
            Status = "New",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // AI summary (optional)
        var summary = await _ai.GenerateSummaryAsync(ticket.Description);
        ticket.Summary = summary;

        await _repo.AddAsync(ticket);

        await _email.SendTicketCreatedAsync(ticket.Email, ticket.Id);

        return MapToResponse(ticket);
    }

    public async Task<TicketResponse?> UpdateAsync(Guid id, UpdateTicketRequest request)
    {
        var ticket = await _repo.GetByIdAsync(id);
        if (ticket is null)
            return null;

   
        var newStatus = string.IsNullOrWhiteSpace(request.Status)
            ? (ticket.Status ?? "New")
            : request.Status;

        bool statusChanged = !string.Equals(ticket.Status, newStatus, StringComparison.OrdinalIgnoreCase);
        bool resolutionChanged = ticket.Resolution != request.Resolution;

        ticket.Status = newStatus;
        ticket.Resolution = request.Resolution;
        ticket.UpdatedAt = DateTime.UtcNow;

        await _repo.UpdateAsync(ticket);

        if (statusChanged)
        {
            
            await _email.SendStatusChangedAsync(ticket.Email, ticket.Id, ticket.Status);
        }

        if (resolutionChanged)
        {
            await _email.SendResolutionChangedAsync(ticket.Email, ticket.Id);
        }
        

        return MapToResponse(ticket);
    }

    private static TicketResponse MapToResponse(Ticket t) =>
        new()
        {
            Id = t.Id,
            Name = t.Name,
            Email = t.Email,
            Description = t.Description,
            Summary = t.Summary,
            ImageUrl = t.ImageUrl,
            Status = t.Status,
            Resolution = t.Resolution,
            CreatedAt = t.CreatedAt,
            UpdatedAt = t.UpdatedAt
        };
}
