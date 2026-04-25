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

    [Fact]
    public async Task GetHomePage_ReturnsSuccess()
    {
        using var factory = CreateFactory();
        var client = factory.CreateClient();

        var response = await client.GetAsync("/");

        response.EnsureSuccessStatusCode();
        var html = await response.Content.ReadAsStringAsync();
        Assert.Contains("Download on Google Play", html);
    }

    [Fact]
    public async Task GetHomePage_ContainsPlayStoreLink()
    {
        using var factory = CreateFactory();
        var client = factory.CreateClient();

        var response = await client.GetAsync("/");

        response.EnsureSuccessStatusCode();
        var html = await response.Content.ReadAsStringAsync();
        Assert.Contains("play.google.com/store/apps/details?id=com.kardsen.ytshortsalarm", html);
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
