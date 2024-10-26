using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SnakeGame.Commands;
using SnakeGame.Hubs;
using SnakeGame.Services;

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
app.UseRouting();
app.MapFallbackToFile("control.html");

app.MapHub<GameHub>("/gamehub");
app.MapDefaultControllerRoute();

app.Run();
