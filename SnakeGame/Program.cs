using SnakeGame.Commands;
using SnakeGame.Hubs;
using SnakeGame.Services;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSignalR();
builder.Services.AddControllersWithViews();

// Register the GameService as a hosted service
builder.Services.AddSingleton<GameService>();
builder.Services.AddSingleton<CommandManager>();
builder.Services.AddHostedService(provider => provider.GetService<GameService>());

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseStaticFiles();

// Serve files from the Sounds directory
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Sounds")),
    RequestPath = "/Sounds"
});

app.UseRouting();
app.MapFallbackToFile("control.html");

app.MapHub<GameHub>("/gamehub");
app.MapDefaultControllerRoute();

app.Run();
