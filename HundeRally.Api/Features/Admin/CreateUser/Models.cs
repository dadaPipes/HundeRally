using FluentValidation;

namespace Admin.CreateUser
{
    public class Request
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<string> Roles { get; set; }
    }

    public class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MinimumLength(2).WithMessage("Name must be at least 2 characters long");


            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long");

            RuleFor(x => x.Roles)
                .NotEmpty().WithMessage("Roles are required")
                .Must(roles => roles != null && roles.All(role => IsValidRole(role)))
                .WithMessage("Roles must be 'Admin', 'Judge', or 'DogHandler'");
        }

        private bool IsValidRole(string role)
        {
            var validRoles = new HashSet<string> { "Admin", "Judge", "DogHandler" };
            return validRoles.Contains(role);
        }
    }

    public class Response
    {
        public string? Message { get; set; }
    }
}
