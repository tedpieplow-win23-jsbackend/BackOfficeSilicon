namespace BackOfficeSilicon.Models;

public class ChatMessage
{
    public string? Message { get; set; }
    public string? UserName { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
}
