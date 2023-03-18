using ChatAppRazor.Hubs;
using ChatAppRazor.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSignalR();
builder.Services.AddDbContext<PRN221_Spr22Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection")));
builder.Services.AddScoped<PRN221_Spr22Context>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllerRoute(
    name: "Employees",
    pattern: "api/Employees",
    defaults: new { controller = "Employees", action = "GetEmployees" }
);
// 2. Cách dưới này cũng được. Nếu làm cách dưới này thì phải thêm Route[] ở controller cho nó.
//app.MapControllerRoute(
//    name: "Employee",
//    pattern: "{controller=Employee}"
//);


app.MapHub<ChatHub>("/chatHub");
app.MapHub<SignalrServer>("/signalrServer");

app.Run();
