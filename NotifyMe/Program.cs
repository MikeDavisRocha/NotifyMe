using Microsoft.EntityFrameworkCore;
using NotifyMe.Data;
using NotifyMe.Services;
using NotifyMe.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IEmailService, SmtpEmailService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // For wwwroot

app.MapPost("/api/notify", async (EmailRequest request, IEmailService emailService) =>
{
    await emailService.SendEmailAsync(request);
    return Results.Ok(new { message = "Email processed" });
});

app.MapGet("/api/history", async (AppDbContext db) =>
{
    var history = await db.EmailLogs
        .OrderByDescending(e => e.SentAt)
        .ToListAsync();
    return Results.Ok(history);
});

app.MapFallbackToFile("index.html");

app.Run();
