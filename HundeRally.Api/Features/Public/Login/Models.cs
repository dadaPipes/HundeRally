using FluentValidation;

namespace Public.Login;

public class Request
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class Validator : Validator<Request>
{
    public Validator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .MinimumLength(2)
            .WithMessage("Email is required");

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(2)
            .WithMessage("Password is required");
    }
}

public class Response
{
    public string? Message { get; set; }
}