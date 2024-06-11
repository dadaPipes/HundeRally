using FluentValidation;

namespace HundeRally.Api.Features.Admin.DeleteUser;

public sealed class Request
{
    public int Id { get; set; }
}

public sealed class Validator : Validator<Request>
{
    public Validator()
    {
        RuleFor(x => x.Id)
            .NotNull().WithMessage("Id is required");
    }
}

public class Response
{
    public string? Message { get; set; }
}
