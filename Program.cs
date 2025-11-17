using System;

class Program
{
    static void Main()
    {
        MusicPlaylist playlist = new MusicPlaylist();
        bool running = true;

        while (running)
        {
            Console.WriteLine("\n--- Music Playlist Manager ---");
            Console.WriteLine("1. Add Song");
            Console.WriteLine("2. Delete Song by Title");
            Console.WriteLine("3. Delete Song by Position");
            Console.WriteLine("4. Display Playlist");
            Console.WriteLine("5. Search Song");
            Console.WriteLine("6. Play All Songs");
            Console.WriteLine("7. Shuffle Playlist");
            Console.WriteLine("8. Sort Playlist (A–Z)");
            Console.WriteLine("9. Total Playlist Duration");
            Console.WriteLine("10. Exit");
            Console.Write("Choose an option: ");

            string option = Console.ReadLine();
            Console.WriteLine();

            switch (option)
            {
                case "1":
                    Console.Write("Title: ");
                    string title = Console.ReadLine();
                    Console.Write("Artist: ");
                    string artist = Console.ReadLine();
                    Console.Write("Album: ");
                    string album = Console.ReadLine();
                    Console.Write("Genre: ");
                    string genre = Console.ReadLine();

                    Console.Write("Duration (mm:ss): ");
                    string durationInput = Console.ReadLine();
                    int durationSeconds = ParseDuration(durationInput);

                    playlist.AddSong(new Song(title, artist, album, genre, durationSeconds));
                    Console.WriteLine("Song added.");
                    break;

                case "2":
                    Console.Write("Enter title: ");
                    if (playlist.DeleteSongByTitle(Console.ReadLine()))
                        Console.WriteLine("Deleted.");
                    else
                        Console.WriteLine("Song not found.");
                    break;

                case "3":
                    Console.Write("Enter position: ");
                    if (int.TryParse(Console.ReadLine(), out int pos) &&
                        playlist.DeleteSongByPosition(pos))
                        Console.WriteLine("Deleted.");
                    else
                        Console.WriteLine("Invalid position.");
                    break;

                case "4":
                    playlist.DisplayAll();
                    break;

                case "5":
                    Console.Write("Search title: ");
                    Song found = playlist.Search(Console.ReadLine());
                    Console.WriteLine(found != null ? found.ToString() : "Not found.");
                    break;

                case "6":
                    playlist.PlayAll();
                    break;

                case "7":
                    playlist.Shuffle();
                    break;

                case "8":
                    playlist.SortByTitle();
                    break;

                case "9":
                    Console.WriteLine("Total Playlist Duration: " + playlist.GetFormattedTotalDuration());
                    break;

                case "10":
                    running = false;
                    break;

                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }

    // Converts mm:ss → seconds
    static int ParseDuration(string input)
    {
        string[] parts = input.Split(':');

        if (parts.Length != 2)
            throw new FormatException("Duration must be in mm:ss format.");

        int minutes = int.Parse(parts[0]);
        int seconds = int.Parse(parts[1]);

        return minutes * 60 + seconds;
    }
}
