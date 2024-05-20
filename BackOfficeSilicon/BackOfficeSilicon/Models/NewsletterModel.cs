namespace BackOfficeSilicon.Models;

public class NewsletterModel()
{
    public string Email { get; set; } = null!;
    public bool IsSubscribed { get; set; }
    public bool DailyNewsLetter { get; set; }
    public bool AdvertisingUpdates { get; set; }
    public bool WeekInReviews { get; set; }
    public bool EventUpdates { get; set; }
    public bool StartupsWeekly { get; set; }
    public bool Podcasts { get; set; }
}
