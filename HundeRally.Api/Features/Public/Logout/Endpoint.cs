using FastEndpoints.Security;

namespace Public.Logout;

public sealed class Endpoint : EndpointWithoutRequest
{
    public override void Configure()
    {
        Post("api/public/logout");
        Roles("Judge", "DogHandler");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await CookieAuth.SignOutAsync();
        await SendNoContentAsync();
    }
}