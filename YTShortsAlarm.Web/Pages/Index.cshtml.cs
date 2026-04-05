using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using YTShortsAlarm.Web.Data;

namespace YTShortsAlarm.Web.Pages;

public class IndexModel : PageModel
{
    private readonly AppDbContext _db;
    private readonly ILogger<IndexModel> _logger;
    private readonly TimeProvider _timeProvider;

    public IndexModel(AppDbContext db, ILogger<IndexModel> logger, TimeProvider timeProvider)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
    }

    [BindProperty]
    public SignupInput Input { get; set; } = new();

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostSignupAsync(CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            TempData["SignupError"] = "Please enter a valid email address.";
            return RedirectToPage(null, null, "download");
        }

        var email = Input.Email!.Trim().ToLowerInvariant();

        var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
        var oneHourAgo = _timeProvider.GetUtcNow().UtcDateTime.AddHours(-1);
        var recentFromIp = await _db.WaitlistEntries
            .CountAsync(e => e.IpAddress == ip && e.CreatedAtUtc > oneHourAgo, cancellationToken);

        if (recentFromIp >= 5)
        {
            TempData["SignupError"] = "Too many signups from this address. Please try again later.";
            return RedirectToPage(null, null, "download");
        }

        var exists = await _db.WaitlistEntries.AnyAsync(e => e.Email == email, cancellationToken);
        if (exists)
        {
            TempData["SignupMessage"] = "You're already on the list! We'll notify you when the app launches.";
            return RedirectToPage(null, null, "download");
        }

        var entry = new WaitlistEntry
        {
            Email = email,
            CreatedAtUtc = _timeProvider.GetUtcNow().UtcDateTime,
            IpAddress = ip
        };

        try
        {
            _db.WaitlistEntries.Add(entry);
            await _db.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            // Unique constraint violation — concurrent duplicate
            TempData["SignupMessage"] = "You're already on the list! We'll notify you when the app launches.";
            return RedirectToPage(null, null, "download");
        }

        _logger.LogInformation("New waitlist signup recorded (Id: {EntryId})", entry.Id);
        TempData["SignupMessage"] = "You're on the list! We'll email you when YT Shorts Alarm launches.";
        return RedirectToPage(null, null, "download");
    }

    public class SignupInput
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [MaxLength(256)]
        public string? Email { get; set; }
    }
}
