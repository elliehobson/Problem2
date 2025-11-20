using System;
using System.Collections.Generic;

//  Music Playlist Manager
// Features:
// - Custom singly linked list (Song Node)
// - Add song (mm:ss input)
// - Delete song (by title and by position)
// - Display all songs
// - Display songs by artist
// - Display songs by duration range
// - Search by title
// - Shuffle (Fisher-Yates using array of nodes -> efficient)
// - Play All (simple simulation)
// - Extra 1: Sort by title (A-Z)
// - Extra 2: Total playlist duration (mm:ss)

class Program
{
    static void Main()
    {
        var playlist = new MusicPlaylist();
        bool running = true;

        Console.WriteLine("Music Playlist Manager (linked list)");
        while (running)
        {
            ShowMenu();
            string? choice = Console.ReadLine();

            Console.WriteLine();
            switch (choice)
            {
                case "1":
                    AddSongFlow(playlist);
                    break;
                case "2":
                    DeleteByTitleFlow(playlist);
                    break;
                case "3":
                    DeleteByPositionFlow(playlist);
                    break;
                case "4":
                    playlist.DisplayAll();
                    break;
                case "5":
                    DisplayByArtistFlow(playlist);
                    break;
                case "6":
                    DisplayByDurationFlow(playlist);
                    break;
                case "7":
                    SearchFlow(playlist);
                    break;
                case "8":
                    playlist.PlayAll();
                    break;
                case "9":
                    playlist.Shuffle();
                    break;
                case "10":
                    playlist.SortByTitle();
                    break;
                case "11":
                    Console.WriteLine("Total Duration: " + playlist.GetFormattedTotalDuration());
                    break;
                case "12":
                    playlist.DisplayCount();
                    break;
                case "0":
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid option. Try again.");
                    break;
            }
            Console.WriteLine();
        }

        Console.WriteLine("Goodbye!");
    }

    static void ShowMenu()
    {
        Console.WriteLine("Menu:");
        Console.WriteLine("1  - Add song");
        Console.WriteLine("2  - Delete song by title");
        Console.WriteLine("3  - Delete song by position");
        Console.WriteLine("4  - Display all songs");
        Console.WriteLine("5  - Display songs by artist");
        Console.WriteLine("6  - Display songs by duration range");
        Console.WriteLine("7  - Search by title");
        Console.WriteLine("8  - Play all");
        Console.WriteLine("9  - Shuffle playlist");
        Console.WriteLine("10 - Sort playlist by title (A-Z)");
        Console.WriteLine("11 - Total playlist duration (mm:ss)");
        Console.WriteLine("12 - Count songs");
        Console.WriteLine("0  - Exit");
        Console.Write("Choose an option: ");
    }

    // user prompts
    static void AddSongFlow(MusicPlaylist playlist)
    {
        Console.Write("Title: ");
        string title = Console.ReadLine() ?? "";

        Console.Write("Artist: ");
        string artist = Console.ReadLine() ?? "";

        Console.Write("Album (optional): ");
        string album = Console.ReadLine() ?? "";

        Console.Write("Genre (optional): ");
        string genre = Console.ReadLine() ?? "";

        Console.Write("Duration (mm:ss): ");
        string durationInput = Console.ReadLine() ?? "";
        int duration = ParseDurationSafe(durationInput);

        playlist.AddSong(new Song(title, artist, album, genre, duration));
        Console.WriteLine("Song added.");
    }

    static void DeleteByTitleFlow(MusicPlaylist playlist)
    {
        Console.Write("Enter title to delete: ");
        string t = Console.ReadLine() ?? "";
        bool ok = playlist.DeleteSongByTitle(t);
        Console.WriteLine(ok ? "Deleted." : "Song not found.");
    }

    static void DeleteByPositionFlow(MusicPlaylist playlist)
    {
        Console.Write("Enter position to delete (1-based): ");
        string? posStr = Console.ReadLine();
        if (int.TryParse(posStr, out int pos))
        {
            bool ok = playlist.DeleteSongByPosition(pos);
            Console.WriteLine(ok ? "Deleted." : "Invalid position.");
        }
        else
        {
            Console.WriteLine("Invalid input.");
        }
    }

    static void DisplayByArtistFlow(MusicPlaylist playlist)
    {
        Console.Write("Artist name: ");
        string artist = Console.ReadLine() ?? "";
        playlist.DisplayByArtist(artist);
    }

    static void DisplayByDurationFlow(MusicPlaylist playlist)
    {
        Console.Write("Min duration (mm:ss): ");
        string minIn = Console.ReadLine() ?? "";
        Console.Write("Max duration (mm:ss): ");
        string maxIn = Console.ReadLine() ?? "";

        int min = ParseDurationSafe(minIn);
        int max = ParseDurationSafe(maxIn);

        if (min > max)
        {
            Console.WriteLine("Min is greater than max. Swapping.");
            int tmp = min; min = max; max = tmp;
        }

        playlist.DisplayByDuration(min, max);
    }

    static void SearchFlow(MusicPlaylist playlist)
    {
        Console.Write("Search title: ");
        string q = Console.ReadLine() ?? "";
        var s = playlist.Search(q);
        Console.WriteLine(s != null ? "Found: " + s.ToString() : "Not found.");
    }

    // accepts mm:ss tolerates mistakes and returns 0 on failure
    static int ParseDurationSafe(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return 0;

        var parts = input.Split(':');
        if (parts.Length != 2)
        {
            Console.WriteLine("Bad format; expected mm:ss. Using 0.");
            return 0;
        }

        if (!int.TryParse(parts[0].Trim(), out int m) || !int.TryParse(parts[1].Trim(), out int s))
        {
            Console.WriteLine("Could not parse numbers. Using 0.");
            return 0;
        }

        if (s < 0 || m < 0)
        {
            Console.WriteLine("Negative not allowed. Using 0.");
            return 0;
        }

        return m * 60 + s;
    }
}

//Song node 
class Song
{
    public string Title { get; set; }
    public string Artist { get; set; }
    public string Album { get; set; }    // optional
    public string Genre { get; set; }    // optional
    public int Duration { get; set; }    // seconds
    public Song? Next { get; set; }      // pointer for singly linked list

    public Song(string title, string artist, string album, string genre, int duration)
    {
        Title = title;
        Artist = artist;
        Album = album;
        Genre = genre;
        Duration = duration;
        Next = null;
    }

    public override string ToString()
    {
        return $"{Title} - {Artist} ({Album}) [{Genre}] {FormatDuration()}";
    }

    public string FormatDuration()
    {
        int m = Duration / 60;
        int s = Duration % 60;
        return $"{m}:{s:D2}";
    }
}

// Playlist (singly linked list)
class MusicPlaylist
{
    private Song? head;

    public MusicPlaylist() { head = null; }

    // Add at end
    public void AddSong(Song song)
    {
        if (head == null)
        {
            head = song;
            return;
        }

        var cur = head;
        while (cur.Next != null) cur = cur.Next;
        cur.Next = song;
    }

    // Delete first match by title notcase sensitive 
    public bool DeleteSongByTitle(string title)
    {
        if (head == null) return false;

        if (string.Equals(head.Title, title, StringComparison.OrdinalIgnoreCase))
        {
            head = head.Next;
            return true;
        }

        var cur = head;
        while (cur.Next != null && !string.Equals(cur.Next.Title, title, StringComparison.OrdinalIgnoreCase))
            cur = cur.Next;

        if (cur.Next == null) return false;

        cur.Next = cur.Next.Next;
        return true;
    }

    // Delete by position
    public bool DeleteSongByPosition(int pos)
    {
        if (pos < 1 || head == null) return false;
        if (pos == 1)
        {
            head = head.Next;
            return true;
        }

        var cur = head;
        for (int i = 1; i < pos - 1; i++)
        {
            if (cur.Next == null) return false;
            cur = cur.Next;
        }

        if (cur.Next == null) return false;
        cur.Next = cur.Next.Next;
        return true;
    }

    // Display all
    public void DisplayAll()
    {
        if (head == null)
        {
            Console.WriteLine("Playlist empty.");
            return;
        }

        int idx = 1;
        var cur = head;
        while (cur != null)
        {
            Console.WriteLine($"{idx++}. {cur}");
            cur = cur.Next;
        }
    }

    // Display by artist not case sensitive
    public void DisplayByArtist(string artist)
    {
        if (head == null)
        {
            Console.WriteLine("Playlist empty.");
            return;
        }

        var cur = head;
        bool found = false;
        while (cur != null)
        {
            if (string.Equals(cur.Artist, artist, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine(cur.ToString());
                found = true;
            }
            cur = cur.Next;
        }
        if (!found) Console.WriteLine("No songs by that artist.");
    }

    // Display by duration range
    public void DisplayByDuration(int minSec, int maxSec)
    {
        if (head == null)
        {
            Console.WriteLine("Playlist empty.");
            return;
        }

        var cur = head;
        bool found = false;
        while (cur != null)
        {
            if (cur.Duration >= minSec && cur.Duration <= maxSec)
            {
                Console.WriteLine(cur.ToString());
                found = true;
            }
            cur = cur.Next;
        }
        if (!found) Console.WriteLine("No songs in that duration range.");
    }

    // Search by title
    public Song? Search(string title)
    {
        var cur = head;
        while (cur != null)
        {
            if (cur.Title.IndexOf(title, StringComparison.OrdinalIgnoreCase) >= 0)
                return cur;
            cur = cur.Next;
        }
        return null;
    }

    // Play all
    public void PlayAll()
    {
        if (head == null)
        {
            Console.WriteLine("Playlist empty.");
            return;
        }

        var cur = head;
        while (cur != null)
        {
            Console.WriteLine("Playing: " + cur.Title + " (" + cur.FormatDuration() + ")");
            System.Threading.Thread.Sleep(400); // short delay for demo
            cur = cur.Next;
        }
    }

    // Shuffle convert nodes to list, shuffle, reassign data to nodes
    public void Shuffle()
    {
        int count = GetCount();
        if (count <= 1)
        {
            Console.WriteLine("Not enough songs to shuffle.");
            return;
        }

        // collect nodes into array
        var nodes = new Song[count];
        var cur = head;
        int i = 0;
        while (cur != null)
        {
            nodes[i++] = cur;
            cur = cur.Next;
        }

        // Fisher-Yates shuffle on the array of Song 
        var rng = new Random();
        for (int a = count - 1; a > 0; a--)
        {
            int b = rng.Next(a + 1);
            // swap entire Song data by swapping fields of the node objects
            SwapNodeData(nodes[a], nodes[b]);
        }

        Console.WriteLine("Playlist shuffled.");
    }

    // Sort by title A-Z
    public void SortByTitle()
    {
        int count = GetCount();
        if (count <= 1)
        {
            Console.WriteLine("Not enough songs to sort.");
            return;
        }

        var nodes = new Song[count];
        var cur = head;
        int i = 0;
        while (cur != null)
        {
            nodes[i++] = cur;
            cur = cur.Next;
        }

        Array.Sort(nodes, (x, y) => string.Compare(x.Title, y.Title, StringComparison.OrdinalIgnoreCase));

        // Reassign data in order
        var sortedData = new List<SongData>(count);
        foreach (var n in nodes)
        {
            sortedData.Add(new SongData(n.Title, n.Artist, n.Album, n.Genre, n.Duration));
        }

        // Walk the list again and overwrite node fields
        cur = head;
        int idx = 0;
        while (cur != null)
        {
            var d = sortedData[idx++];
            cur.Title = d.Title;
            cur.Artist = d.Artist;
            cur.Album = d.Album;
            cur.Genre = d.Genre;
            cur.Duration = d.Duration;
            cur = cur.Next;
        }

        Console.WriteLine("Playlist sorted A–Z by title.");
    }

    // Get total seconds for all songs
    public int GetTotalDurationSeconds()
    {
        int total = 0;
        var cur = head;
        while (cur != null)
        {
            total += cur.Duration;
            cur = cur.Next;
        }
        return total;
    }

    public string GetFormattedTotalDuration()
    {
        int total = GetTotalDurationSeconds();
        int m = total / 60;
        int s = total % 60;
        return $"{m}:{s:D2}";
    }

    public void DisplayCount()
    {
        Console.WriteLine("Songs in playlist: " + GetCount());
    }

    
    private int GetCount()
    {
        int c = 0;
        var cur = head;
        while (cur != null) { c++; cur = cur.Next; }
        return c;
    }

    // Swaps the data of two node objects
    private void SwapNodeData(Song a, Song b)
    {
        // Swap fields (not the Next pointer)
        var tmpTitle = a.Title;
        var tmpArtist = a.Artist;
        var tmpAlbum = a.Album;
        var tmpGenre = a.Genre;
        var tmpDuration = a.Duration;

        a.Title = b.Title;
        a.Artist = b.Artist;
        a.Album = b.Album;
        a.Genre = b.Genre;
        a.Duration = b.Duration;

        b.Title = tmpTitle;
        b.Artist = tmpArtist;
        b.Album = tmpAlbum;
        b.Genre = tmpGenre;
        b.Duration = tmpDuration;
    }

    //to hold song fields temporarily for sorted reassign
    private struct SongData
    {
        public string Title, Artist, Album, Genre;
        public int Duration;
        public SongData(string t, string a, string al, string g, int d)
        {
            Title = t; Artist = a; Album = al; Genre = g; Duration = d;
        }
    }
}

