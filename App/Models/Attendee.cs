namespace App.Models;

public class Attendee
{
    public EmailAddress EmailAddress { get; set; }
    public string Type { get; set; }
    public Status Status { get; set; }
}