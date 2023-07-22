namespace DriveLinker.Models;

public class Message
{
    public Drive Drive { get; set; }

    public Message(Drive drive)
    {
        Drive = drive;
    }
}
