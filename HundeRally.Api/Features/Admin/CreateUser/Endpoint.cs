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
            AddError("User with that email does already exist");  // TODO: Send HTTP 409 Conflict
        
        ThrowIfAnyErrors();

        await data.CreateUserAsync(Map.ToEntity(req));

        await SendAsync(new() { Message = "User successfully created" });
    }
}