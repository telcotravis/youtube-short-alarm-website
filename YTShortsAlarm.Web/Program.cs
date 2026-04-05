using Microsoft.EntityFrameworkCore;
using YTShortsAlarm.Web.Data;

var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString("DefaultConnection")!;
if (connString.Contains("App_Data"))
{
    var dataDir = Path.Combine(builder.Environment.ContentRootPath, "App_Data");
    Directory.CreateDirectory(dataDir);
    connString = connString.Replace("App_Data\\", dataDir + Path.DirectorySeparatorChar);
}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connString));

builder.Services.AddRazorPages();
builder.Services.AddAntiforgery();
builder.Services.AddSingleton(TimeProvider.System);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.EnsureCreatedAsync();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAntiforgery();
app.MapRazorPages();

app.Run();

public partial class Program { }
