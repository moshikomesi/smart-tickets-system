using System.Text.Json;
using SupportTickets.Api.Domain;

namespace SupportTickets.Api.Repositories;

public class JsonTicketRepository : ITicketRepository
{
    private readonly string _filePath;

    public JsonTicketRepository(IWebHostEnvironment env)
    {

        _filePath = Path.Combine(env.ContentRootPath, "tickets.json");
    }

    public async Task<List<Ticket>> GetAllAsync()
    {
        if (!File.Exists(_filePath))
        {
            return new List<Ticket>();
        }

        var json = await File.ReadAllTextAsync(_filePath);

        if (string.IsNullOrWhiteSpace(json))
            return new List<Ticket>();

        var tickets = JsonSerializer.Deserialize<List<Ticket>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true // for matching JSON property names
        });

        return tickets ?? new List<Ticket>();
    }

    public async Task<Ticket?> GetByIdAsync(Guid id)
    {
        var tickets = await GetAllAsync();
        return tickets.FirstOrDefault(t => t.Id == id);
    }

    public async Task AddAsync(Ticket ticket)
    {
        var tickets = await GetAllAsync();
        tickets.Add(ticket);
        await SaveAllAsync(tickets);
    }

    public async Task UpdateAsync(Ticket ticket)
    {
        var tickets = await GetAllAsync();

        var index = tickets.FindIndex(t => t.Id == ticket.Id);
        if (index == -1)
        {
            throw new InvalidOperationException("Ticket not found");
        }

        tickets[index] = ticket;

        await SaveAllAsync(tickets);
    }

    private async Task SaveAllAsync(List<Ticket> tickets)
    {
        var json = JsonSerializer.Serialize(tickets, new JsonSerializerOptions
        {
            WriteIndented = true // for better readability in the JSON file
        });

        await File.WriteAllTextAsync(_filePath, json);
    }
}
