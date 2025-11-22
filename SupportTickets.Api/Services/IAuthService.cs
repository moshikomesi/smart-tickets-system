namespace SupportTickets.Api.Services;

public interface IAuthService
{
    string? Login(string username, string password);
    string GuestLogin();
}
