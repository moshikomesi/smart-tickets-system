using SupportTickets.Api.DTOs;

namespace SupportTickets.Api.Services;

public interface ITicketService
{
    Task<List<TicketResponse>> GetAllAsync(string? status, string? search);// for listing with filters
    Task<TicketResponse?> GetByIdAsync(Guid id);// for detail view
    Task<TicketResponse> CreateAsync(CreateTicketRequest request);// for creating new ticket
    Task<TicketResponse?> UpdateAsync(Guid id, UpdateTicketRequest request);// for updating existing ticket
}

