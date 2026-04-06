using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using YTShortsAlarm.Web.Data;

namespace YTShortsAlarm.Web.Pages.Admin;

public class WaitlistModel : PageModel
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _config;

    public WaitlistModel(AppDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    public List<WaitlistEntry> Entries { get; set; } = [];
    public string Key { get; set; } = "";

    public async Task<IActionResult> OnGetAsync(string? key, CancellationToken ct)
    {
        if (!IsValidKey(key))
            return NotFound();

        Key = key!;
        Entries = await _db.WaitlistEntries
            .OrderByDescending(e => e.CreatedAtUtc)
            .ToListAsync(ct);

        return Page();
    }

    public async Task<IActionResult> OnGetExportCsvAsync(string? key, CancellationToken ct)
    {
        if (!IsValidKey(key))
            return NotFound();

        var entries = await _db.WaitlistEntries
            .OrderByDescending(e => e.CreatedAtUtc)
            .ToListAsync(ct);

        var sb = new StringBuilder();
        sb.AppendLine("Email,SignedUpUtc");
        foreach (var entry in entries)
        {
            // Escape emails that might contain commas (unlikely but safe)
            sb.AppendLine($"\"{entry.Email}\",\"{entry.CreatedAtUtc:yyyy-MM-dd HH:mm:ss}\"");
        }

        return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "waitlist.csv");
    }

    private bool IsValidKey(string? key)
    {
        var expected = _config["AdminKey"];
        return !string.IsNullOrEmpty(expected)
            && string.Equals(key, expected, StringComparison.Ordinal);
    }
}
