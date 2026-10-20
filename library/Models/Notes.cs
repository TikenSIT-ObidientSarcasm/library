namespace library.Models;

// One Note object holds one row of lending.book.
public class Note
{
    public long note_id { get; set; }

    public string title { get; set; } = "";

    public string body { get; set; } = "";

    public string? created_on { get; set; }
}
