using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using HundeRally.Api.Data;
using HundeRally.Api.Models;

namespace HundeRally.Tests;

public class Sut : AppFixture<Program>
{
    protected override Task SetupAsync()
    {
        using (var scope = Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<HundeRallyDbContext>();

            // Define a default password for all users
            const string defaultPassword = "Passw0rd!";
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(defaultPassword);

            // create users


            // Create users
            var admin = new Administrator
            {
                Email = "Admin@example.com",
                Name = "Admin",
                PasswordHash = passwordHash };

            var judge = new Judge
            { Email = "Judge@example.com",
                Name = "Judge",
                PasswordHash = passwordHash };

            var dogHandler = new DogHandler
            {
                Email = "DogHandler@example.com",
                Name = "DogHandler",
                PasswordHash = passwordHash };

            dbContext.Users.AddRange(admin, judge, dogHandler);
            dbContext.SaveChanges();
        }

        return Task.CompletedTask;
    }

    protected override void ConfigureApp(IWebHostBuilder a)
    {
        // Host builder configuration
    }

    protected override void ConfigureServices(IServiceCollection s)
    {
        s.AddDbContext<HundeRallyDbContext>(options =>
            options.UseInMemoryDatabase("TestDb"));
    }

    protected override async Task TearDownAsync()
    {
        using (var scope = Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<HundeRallyDbContext>();
            dbContext.Database.EnsureDeleted();
        }
        await Task.CompletedTask;
    }
}
