using Microsoft.AspNetCore.Mvc;

namespace Admin.CreateUser;

public sealed class Endpoint(Data data) : Endpoint<Request, Response, Mapper>
{
    public override void Configure()
    {
        Post("api/admin/create-user");
        Roles("Admin");
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        if (await data.UserAlreadyExistsAsync(req.Email))
        {
            var response = new Response { Message = "User with that email does already exist" };
            await SendAsync(response, 409);
        }

        ThrowIfAnyErrors();

        await data.CreateUserAsync(Map.ToEntity(req));

        await SendAsync(new() { Message = "User successfully created" });
    }
}