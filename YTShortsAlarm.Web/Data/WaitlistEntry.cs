namespace YTShortsAlarm.Web.Data;

public class WaitlistEntry
{
    public int Id { get; set; }
    public required string Email { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public string? IpAddress { get; set; }
}
