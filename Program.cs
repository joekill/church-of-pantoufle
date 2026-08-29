using Microsoft.EntityFrameworkCore;
using ChurchOfPantoufle.Data;
using ChurchOfPantoufle.Services;

var builder = WebApplication.CreateBuilder(args);

var port = Environment.GetEnvironmentVariable("PORT");
if (port != null)
{
    builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
}

builder.Services.AddControllersWithViews();
var dbFolder = Environment.GetEnvironmentVariable("HOME") is string home
    ? Path.Combine(home, "data")
    : ".";
Directory.CreateDirectory(dbFolder);
var dbPath = Path.Combine(dbFolder, "church.db");
builder.Services.AddDbContext<ChurchContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));
builder.Services.AddScoped<IDonationService, MockDonationService>();

var app = builder.Build();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ChurchContext>();
    SeedData.Initialize(context);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
