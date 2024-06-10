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
        // make a data call to see if email already exists

        await data.CreateUserAsync(Map.ToEntity(req));

        await SendAsync(new() { Message = "User successfully created" });
    }
}