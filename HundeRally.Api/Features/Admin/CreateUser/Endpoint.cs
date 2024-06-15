namespace Admin.CreateUser;

public sealed class Endpoint(Data data) : Endpoint<Request, Response, Mapper>
{
    public override void Configure()
    {
        Post("api/admin/user/create");
        Roles("Admin");
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        if (await data.UserExistsAsync(req.Email))
            AddError("User with that email does already exist");

        ThrowIfAnyErrors();

        await data.CreateUserAsync(Map.ToEntity(req));

        await SendAsync(new() { Message = "User successfully created" });
    }
}