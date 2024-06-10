using FastEndpoints.Security;
using HundeRally.Api.Features.Public.Login;

namespace Public.Login;

public sealed class Endpoint(Data data) : Endpoint<Request, Response, Mapper>
{
    public override void Configure()
    {
        Post("api/public/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var user = await data.GetUserByEmailAsync(req.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash))
            AddError("Invalid login details.");

        ThrowIfAnyErrors();

        await CookieAuth.SignInAsync(
            u =>
            {
                u.Roles.AddRange(user.Roles);
                u["Email"] = user.Email;
                u["UserName"] = user.Name;
            });

        await SendAsync(new(){Message = $"Welcome {user!.Name}"});
    }
}