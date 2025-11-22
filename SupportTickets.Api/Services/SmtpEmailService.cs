using System.Net;
using System.Net.Mail;

namespace SupportTickets.Api.Services;

public class SmtpEmailService : IEmailService
{
    private readonly EmailSettings _settings;

    public SmtpEmailService(IConfiguration config)
    {
        _settings = config.GetSection("Email").Get<EmailSettings>()
                    ?? throw new InvalidOperationException("Email settings are missing");
    }

    private SmtpClient CreateClient()
    {
        var client = new SmtpClient(_settings.SmtpHost, _settings.SmtpPort)
        {
            EnableSsl = _settings.UseSsl,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(_settings.User, _settings.Password)
        };

        return client;
    }

    public async Task SendTicketCreatedAsync(string to, Guid ticketId)
    {
        var subject = $"🎫 Ticket created: {ticketId}";
        var body = $@"
Thank you for contacting Smart Support!

Your ticket has been created.
Tracking ID: {ticketId}

We will update you as soon as the status changes.

Best regards,
Smart Support Team
";

        await SendEmailAsync(to, subject, body);
    }

    public async Task SendStatusChangedAsync(string to, Guid ticketId, string newStatus)
    {
        var subject = $"🔄 Ticket {ticketId} status updated to {newStatus}";
        var body = $@"
Hi,

The status of your ticket ({ticketId}) has been updated to: {newStatus}.

You can continue to follow up via our support portal.

Best regards,
Smart Support Team
";

        await SendEmailAsync(to, subject, body);
    }

    public async Task SendResolutionChangedAsync(string to, Guid ticketId)
    {
        var subject = $"✅ Ticket {ticketId} resolution updated";
        var body = $@"
Hi,

The resolution text for your ticket ({ticketId}) has been updated.

If the issue is not fully resolved, feel free to reply or open a new ticket.

Best regards,
Smart Support Team
";

        await SendEmailAsync(to, subject, body);
    }

    private async Task SendEmailAsync(string to, string subject, string body)
    {
        using var client = CreateClient();
        using var message = new MailMessage
        {
            From = new MailAddress(_settings.From),
            Subject = subject,
            Body = body,
            IsBodyHtml = false
        };

        message.To.Add(to);

        await client.SendMailAsync(message);
    }
}
