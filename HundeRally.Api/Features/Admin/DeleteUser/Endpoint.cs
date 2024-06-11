namespace HundeRally.Api.Features.Admin.DeleteUser;

public sealed class Endpoint(Data data) : Endpoint<Request, Response, Mapper>
{
    public override void Configure()
    {
        Delete("api/admin/delete-user");
        Roles("Admin");
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var user = await data.GetUserByIdAsync(req.Id);

        if (user is null)
            AddError("User not found");

        ThrowIfAnyErrors();

        await data.DeleteUserByIdAsync(user);
        await SendAsync(new Response { Message = "User succesfully deleted" });
    }
}