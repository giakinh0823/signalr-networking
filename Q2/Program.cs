using Microsoft.EntityFrameworkCore;
using Q2.Hubs;
using Q2.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddSignalR();

builder.Services.AddRazorPages();
builder.Services.AddDbContext<PE_PRN_Fall22B1Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection")));
builder.Services.AddScoped<PE_PRN_Fall22B1Context>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.MapGet("/", () => "Hello World!");

app.MapHub<SignalRhub>("/signalrServer");

app.MapRazorPages();

app.MapControllerRoute(
    name: "Movie",
    pattern: "{controller=Movie}"
);

app.MapControllerRoute(
    name: "Star",
    pattern: "{controller=Star}"
);
app.Run();


