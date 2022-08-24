namespace App.Models;

public class Event
{
    public string Subject { get; set; }
    public Date Start { get; set; }
    public Date End { get; set; }
    public Location Location { get; set; }
    public string BodyPreview { get; set; }
    public List<Attendee> Attendees { get; set; }
    public string OnlineMeetingUrl { get; set; }
    public bool IsOnlineMeeting { get; set; }
    public OnlineMeeting OnlineMeeting { get; set; }
    public string OnlineMeetingProvider { get; set; }
    public string Id { get; set; }
    public Organizer Organizer { get; set; }
}