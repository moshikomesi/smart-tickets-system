using SupportTickets.Api.DTOs;
using SupportTickets.Api.Services;

namespace SupportTickets.Api.Endpoints;

public static class TicketEndpoints
{
    public static IEndpointRouteBuilder MapTicketEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/tickets");

        // GET /api/tickets?status=&search=
        group.MapGet("/", async (ITicketService service, string? status, string? search) =>
        {
            var result = await service.GetAllAsync(status, search);
            return Results.Ok(result);
        });

        // GET /api/tickets/{id}
        group.MapGet("/{id:guid}", async (ITicketService service, Guid id) =>
        {
            var ticket = await service.GetByIdAsync(id);
            return ticket is null ? Results.NotFound() : Results.Ok(ticket);
        });

        // POST /api/tickets
        group.MapPost("/", async (ITicketService service, CreateTicketRequest request) =>
        {
            var ticket = await service.CreateAsync(request);
            return Results.Created($"/api/tickets/{ticket.Id}", ticket);
        });

        // PUT /api/tickets/{id}
        group.MapPut("/{id:guid}", async (ITicketService service, Guid id, UpdateTicketRequest request) =>
        {
            var ticket = await service.UpdateAsync(id, request);
            return ticket is null ? Results.NotFound() : Results.Ok(ticket);
        }).RequireAuthorization();

        return app;
    }
}
