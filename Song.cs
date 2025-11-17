public class Song
{
    public string Title { get; set; }
    public string Artist { get; set; }
    public string Album { get; set; }
    public string Genre { get; set; }
    public int Duration { get; set; }  // stored as seconds

    public Song(string title, string artist, string album, string genre, int duration)
    {
        Title = title;
        Artist = artist;
        Album = album;
        Genre = genre;
        Duration = duration;
    }

    private string FormatDuration()
    {
        int minutes = Duration / 60;
        int seconds = Duration % 60;
        return $"{minutes}:{seconds:D2}";
    }

    public override string ToString()
    {
        return $"{Title} - {Artist} ({Album}) [{Genre}]  {FormatDuration()}";
    }
}
