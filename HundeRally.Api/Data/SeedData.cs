using HundeRally.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace HundeRally.Api.Data;

public static class SeedData
{
    static readonly IEnumerable<UserBase> seedUsers =
    [
        new()
        {
            Email = "leela@contoso.com",
            Name = "Leela",
            Roles = ["Admin", "Judge", "DogHandler" ]
        },
        new()
        {
            Email = "harry@contoso.com",
            Name = "Harry",
            Roles = ["DogHandler"]
        }
    ];

    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        await using var context = new HundeRallyDbContext(serviceProvider.GetRequiredService<DbContextOptions<HundeRallyDbContext>>());

        if (context.Users.Any())
            return;

        foreach (var user in seedUsers)
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword("Passw0rd!");
            context.Users.Add(user);
        }

        await context.SaveChangesAsync();
    }
}