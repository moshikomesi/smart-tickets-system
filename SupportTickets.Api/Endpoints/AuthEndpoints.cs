using SupportTickets.Api.Services;

namespace SupportTickets.Api.Endpoints;

public static class AuthEndpoints
{
    public record LoginRequest(string Username, string Password);
    public record LoginResponse(string Token);

    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth");

        group.MapPost("/login", (IAuthService auth, LoginRequest req) =>
        {
            var token = auth.Login(req.Username, req.Password);
            if (token is null)
                return Results.Unauthorized();

            return Results.Ok(new LoginResponse(token));
        });

        group.MapPost("/guest", (IAuthService auth) =>
{
    var token = auth.GuestLogin();
    return Results.Ok(new { token });
});
        

        return app;
    }
}
