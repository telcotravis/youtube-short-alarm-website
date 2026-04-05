using System.Net;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using YTShortsAlarm.Web.Data;

namespace YTShortsAlarm.Web.Tests;

public class IntegrationTests
{
    private static WebApplicationFactory<Program> CreateFactory()
    {
        return new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var toRemove = services.Where(d =>
                    d.ServiceType == typeof(DbContextOptions<AppDbContext>) ||
                    d.ServiceType == typeof(DbContextOptions) ||
                    d.ServiceType == typeof(AppDbContext))
                    .ToList();
                foreach (var d in toRemove)
                    services.Remove(d);

                var dbName = Guid.NewGuid().ToString();
                services.AddScoped(_ =>
                {
                    var options = new DbContextOptionsBuilder<AppDbContext>()
                        .UseInMemoryDatabase(dbName)
                        .Options;
                    return new AppDbContext(options);
                });
            });
        });
    }

    private static async Task<string> GetAntiforgeryTokenAsync(HttpClient client)
    {
        var response = await client.GetAsync("/");
        response.EnsureSuccessStatusCode();
        var html = await response.Content.ReadAsStringAsync();

        var match = Regex.Match(html,
            @"name=""__RequestVerificationToken""\s+type=""hidden""\s+value=""([^""]+)""");
        Assert.True(match.Success, "Antiforgery token not found in HTML");
        return match.Groups[1].Value;
    }

    private static async Task<HttpResponseMessage> PostSignupAsync(
        HttpClient client, string email, string antiforgeryToken)
    {
        return await client.PostAsync("/?handler=Signup",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "Input.Email", email },
                { "__RequestVerificationToken", antiforgeryToken }
            }));
    }

    [Fact]
    public async Task GetHomePage_ReturnsSuccess()
    {
        using var factory = CreateFactory();
        var client = factory.CreateClient();

        var response = await client.GetAsync("/");

        response.EnsureSuccessStatusCode();
        var html = await response.Content.ReadAsStringAsync();
        Assert.Contains("Wake up to your favorite", html);
    }

    [Fact]
    public async Task GetPrivacyPage_ReturnsSuccess()
    {
        using var factory = CreateFactory();
        var client = factory.CreateClient();

        var response = await client.GetAsync("/Privacy");

        response.EnsureSuccessStatusCode();
        var html = await response.Content.ReadAsStringAsync();
        Assert.Contains("Privacy Policy", html);
    }

    [Fact]
    public async Task GetPrivacyPage_ContainsYouTubeAttribution()
    {
        using var factory = CreateFactory();
        var client = factory.CreateClient();

        var response = await client.GetAsync("/Privacy");

        response.EnsureSuccessStatusCode();
        var html = await response.Content.ReadAsStringAsync();
        Assert.Contains("YouTube API Services", html);
    }

    [Fact]
    public async Task PostSignup_ValidEmail_StoresEntry()
    {
        using var factory = CreateFactory();
        var client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        var token = await GetAntiforgeryTokenAsync(client);
        var response = await PostSignupAsync(client, "test@example.com", token);

        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);

        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var entry = await db.WaitlistEntries.FirstOrDefaultAsync(e => e.Email == "test@example.com");
        Assert.NotNull(entry);
    }

    [Fact]
    public async Task PostSignup_InvalidEmail_RejectsEntry()
    {
        using var factory = CreateFactory();
        var client = factory.CreateClient();

        var token = await GetAntiforgeryTokenAsync(client);
        var response = await PostSignupAsync(client, "not-an-email", token);

        response.EnsureSuccessStatusCode();
        var html = await response.Content.ReadAsStringAsync();
        Assert.Contains("Please enter a valid email address", html);
    }

    [Fact]
    public async Task PostSignup_DuplicateEmail_ReturnsFriendlyMessage()
    {
        using var factory = CreateFactory();

        using (var scope = factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.WaitlistEntries.Add(new WaitlistEntry
            {
                Email = "existing@example.com",
                CreatedAtUtc = DateTime.UtcNow,
                IpAddress = null
            });
            await db.SaveChangesAsync();
        }

        var client = factory.CreateClient();
        var token = await GetAntiforgeryTokenAsync(client);
        var response = await PostSignupAsync(client, "existing@example.com", token);

        response.EnsureSuccessStatusCode();
        var html = await response.Content.ReadAsStringAsync();
        Assert.Contains("already on the list", html);
    }

    [Fact]
    public async Task PostSignup_RateLimited_RejectsAfterFiveSignups()
    {
        using var factory = CreateFactory();
        var client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        var token = await GetAntiforgeryTokenAsync(client);

        for (int i = 1; i <= 5; i++)
        {
            var response = await PostSignupAsync(client, $"user{i}@example.com", token);
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        }

        var sixthResponse = await PostSignupAsync(client, "user6@example.com", token);
        Assert.Equal(HttpStatusCode.Redirect, sixthResponse.StatusCode);

        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        Assert.Equal(5, await db.WaitlistEntries.CountAsync());
        Assert.False(await db.WaitlistEntries.AnyAsync(e => e.Email == "user6@example.com"));
    }

    [Fact]
    public async Task GetRobotsTxt_ReturnsContent()
    {
        using var factory = CreateFactory();
        var client = factory.CreateClient();

        var response = await client.GetAsync("/robots.txt");

        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Sitemap:", content);
    }

    [Fact]
    public async Task GetSitemapXml_ReturnsContent()
    {
        using var factory = CreateFactory();
        var client = factory.CreateClient();

        var response = await client.GetAsync("/sitemap.xml");

        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("<?xml", content);
        Assert.Contains("<urlset", content);
    }
}
