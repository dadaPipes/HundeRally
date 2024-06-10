using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using HundeRally.Api.Data;
namespace HundeRally.Tests;

public class Sut : AppFixture<Program>
{
    protected override Task SetupAsync()
    {
        using (var scope = Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<HundeRallyDbContext>();
            
            dbContext.Users.Add(
                new()
                {
                    Email = "Judge@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Passw0rd!"),
                    Name = "Judge",
                    Roles = ["Judge"]
                });

            dbContext.Users.Add(
                new()
                {
                    Email = "Admin@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Passw0rd!"),
                    Name = "Admin",
                    Roles = ["Admin"]
                });

            dbContext.SaveChanges();
        }

        return Task.CompletedTask;
    }

    protected override void ConfigureApp(IWebHostBuilder a)
    {
        // do host builder configuration here
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