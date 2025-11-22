using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SupportTickets.Api.Domain;
using SupportTickets.Api.DTOs;
using SupportTickets.Api.Repositories;
using SupportTickets.Api.Services;
using Xunit;

namespace SupportTickets.Tests;

public class TicketServiceTests
{
    private class InMemoryTicketRepository : ITicketRepository
    {
        public List<Ticket> Tickets { get; } = new();

        public Task<List<Ticket>> GetAllAsync() => Task.FromResult(new List<Ticket>(Tickets));

        public Task<Ticket?> GetByIdAsync(Guid id) =>
            Task.FromResult<Ticket?>(Tickets.Find(t => t.Id == id));

        public Task AddAsync(Ticket ticket)
        {
            Tickets.Add(ticket);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Ticket ticket)
        {
            var index = Tickets.FindIndex(t => t.Id == ticket.Id);
            if (index >= 0)
                Tickets[index] = ticket;
            return Task.CompletedTask;
        }
    }

    private class FakeEmailService : IEmailService
    {
        public int CreatedEmailsCount { get; private set; }
        public int StatusEmailsCount { get; private set; }
        public int ResolutionEmailsCount { get; private set; }

        public Task SendTicketCreatedAsync(string to, Guid ticketId)
        {
            CreatedEmailsCount++;
            return Task.CompletedTask;
        }

        public Task SendStatusChangedAsync(string to, Guid ticketId, string newStatus)
        {
            StatusEmailsCount++;
            return Task.CompletedTask;
        }

        public Task SendResolutionChangedAsync(string to, Guid ticketId)
        {
            ResolutionEmailsCount++;
            return Task.CompletedTask;
        }
    }

    private class FakeAiService : IAiSummaryService
    {
        public Task<string?> GenerateSummaryAsync(string description)
        {
            return Task.FromResult<string?>($"SUMMARY: {description}");
        }
    }

    [Fact]
    public async Task CreateAsync_CreatesTicketWithNewStatusAndSendsEmail()
    {
        // Arrange
        var repo = new InMemoryTicketRepository();
        var email = new FakeEmailService();
        var ai = new FakeAiService();

        var service = new TicketService(repo, email, ai);

        var request = new CreateTicketRequest
        {
            Name = "John Doe",
            Email = "john@example.com",
            Description = "My laptop is overheating",
            ImageUrl = null
        };

        // Act
        var result = await service.CreateAsync(request);

        // Assert
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal("New", result.Status);
        Assert.StartsWith("SUMMARY:", result.Summary);
        Assert.Single(repo.Tickets);
        Assert.Equal(1, email.CreatedEmailsCount);
    }

    [Fact]
    public async Task UpdateAsync_WhenStatusChanges_SendsStatusEmail()
    {
        // Arrange
        var repo = new InMemoryTicketRepository();
        var email = new FakeEmailService();
        var ai = new FakeAiService();
        var service = new TicketService(repo, email, ai);

        var existing = new Ticket
        {
            Id = Guid.NewGuid(),
            Name = "John",
            Email = "john@example.com",
            Description = "Test",
            Status = "New",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        repo.Tickets.Add(existing);

        var updateRequest = new UpdateTicketRequest
        {
            Status = "In Progress",
            Resolution = null
        };

        // Act
        var result = await service.UpdateAsync(existing.Id, updateRequest);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("In Progress", result!.Status);
        Assert.Equal(1, email.StatusEmailsCount);
        Assert.Equal(0, email.ResolutionEmailsCount);
    }
}
