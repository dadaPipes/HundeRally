using HundeRally.Api.Models;

namespace HundeRally.Api.Features.Admin.DeleteUser;

public sealed class Mapper : Mapper<Request, Response, UserBase>
{
    public override UserBase ToEntity(Request req)
    => new()
    {
        Id = req.Id
    };
}