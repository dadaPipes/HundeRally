global using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using HundeRally.Api;
using HundeRally.Api.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Link: https://github.com/FastEndpoints/FastEndpoints/issues/389
builder.Services
    .AddAuthenticationCookie(validFor: TimeSpan.FromMinutes(60), o =>
    {
        o.SlidingExpiration = true;

        o.Events = new CookieAuthenticationEvents()
        {
            // Handle redirection for login attempts
            OnRedirectToLogin = (ctx) =>
            {
                // Check if the request is for an API endpoint
                if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode == 200)
                {
                    // Instead of redirecting, return a 401 Unauthorized status code
                    ctx.Response.StatusCode = 401;
                }
                return Task.CompletedTask;
            },
            // Handle redirection when access is denied
            OnRedirectToAccessDenied = (ctx) =>
            {
                // Check if the request is for an API endpoint
                if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode == 200)
                {
                    // Instead of redirecting, return a 403 Forbidden status code
                    ctx.Response.StatusCode = 403;
                }
                return Task.CompletedTask;
            }
        };
    });


builder.Services
       .AddAuthorization()
       .AddFastEndpoints()
       .SwaggerDocument()
       .RegisterServicesFromHundeRallyApi();

builder.Services.AddDbContext<HundeRallyDbContext>(
    options =>
    {
        options.UseInMemoryDatabase("AppDb");
        options.EnableDetailedErrors();
        options.EnableSensitiveDataLogging();
    });

builder.Services.AddCors(
    options => options.AddPolicy(
        "wasm",
        policy =>
            policy.WithOrigins("https://localhost:7171")
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials()));

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.UseFastEndpoints();
app.UseSwaggerGen();

// seed database
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
await SeedData.InitializeAsync(services);

app.Run();

// Make the implicit Program class public so test projects can access it
public partial class Program;