using System;

public class MusicPlaylist
{
    private Node head;

    public MusicPlaylist()
    {
        head = null;
    }

    // ------------------ ADD SONG ------------------
    public void AddSong(Song song)
    {
        Node newNode = new Node(song);

        if (head == null)
        {
            head = newNode;
            return;
        }

        Node current = head;
        while (current.Next != null)
            current = current.Next;

        current.Next = newNode;
    }

    // ------------------ DELETE BY TITLE ------------------
    public bool DeleteSongByTitle(string title)
    {
        if (head == null)
            return false;

        if (head.Data.Title.Equals(title, StringComparison.OrdinalIgnoreCase))
        {
            head = head.Next;
            return true;
        }

        Node current = head;
        while (current.Next != null &&
              !current.Next.Data.Title.Equals(title, StringComparison.OrdinalIgnoreCase))
        {
            current = current.Next;
        }

        if (current.Next == null)
            return false;

        current.Next = current.Next.Next;
        return true;
    }

    // ------------------ DELETE BY POSITION ------------------
    public bool DeleteSongByPosition(int position)
    {
        if (position < 1 || head == null)
            return false;

        if (position == 1)
        {
            head = head.Next;
            return true;
        }

        Node current = head;
        for (int i = 1; i < position - 1; i++)
        {
            if (current == null)
                return false;
            current = current.Next;
        }

        if (current?.Next == null)
            return false;

        current.Next = current.Next.Next;
        return true;
    }

    // ------------------ DISPLAY ALL SONGS ------------------
    public void DisplayAll()
    {
        if (head == null)
        {
            Console.WriteLine("Playlist is empty.");
            return;
        }

        Node current = head;
        int index = 1;

        while (current != null)
        {
            Console.WriteLine($"{index++}. {current.Data}");
            current = current.Next;
        }
    }

    // ------------------ SEARCH SONG ------------------
    public Song Search(string title)
    {
        Node current = head;

        while (current != null)
        {
            if (current.Data.Title.Equals(title, StringComparison.OrdinalIgnoreCase))
                return current.Data;

            current = current.Next;
        }

        return null;
    }

    // ------------------ PLAY ALL SONGS ------------------
    public void PlayAll()
    {
        if (head == null)
        {
            Console.WriteLine("Playlist empty.");
            return;
        }

        Node current = head;

        while (current != null)
        {
            Console.WriteLine($"Playing: {current.Data.Title}...");
            System.Threading.Thread.Sleep(400);
            current = current.Next;
        }
    }

    // ------------------ SHUFFLE ------------------
    public void Shuffle()
    {
        int count = GetCount();
        if (count <= 1) return;

        Random rnd = new Random();

        for (int i = 0; i < count; i++)
        {
            int r = rnd.Next(i, count);

            Node a = GetNodeAt(i);
            Node b = GetNodeAt(r);

            Song temp = a.Data;
            a.Data = b.Data;
            b.Data = temp;
        }

        Console.WriteLine("Playlist shuffled.");
    }

    // ------------------ EXTRA FEATURE 1: SORT A–Z ------------------
    public void SortByTitle()
    {
        if (head == null || head.Next == null)
        {
            Console.WriteLine("Not enough songs to sort.");
            return;
        }

        bool swapped;
        do
        {
            swapped = false;
            Node current = head;

            while (current.Next != null)
            {
                if (string.Compare(current.Data.Title, current.Next.Data.Title,
                    StringComparison.OrdinalIgnoreCase) > 0)
                {
                    Song temp = current.Data;
                    current.Data = current.Next.Data;
                    current.Next.Data = temp;
                    swapped = true;
                }
                current = current.Next;
            }
        }
        while (swapped);

        Console.WriteLine("Playlist sorted A–Z by title.");
    }

    // ------------------ EXTRA FEATURE 2: TOTAL DURATION ------------------
    public int GetTotalDuration()
    {
        int total = 0;
        Node current = head;

        while (current != null)
        {
            total += current.Data.Duration;
            current = current.Next;
        }

        return total;
    }

    public string GetFormattedTotalDuration()
    {
        int total = GetTotalDuration();
        int minutes = total / 60;
        int seconds = total % 60;

        return $"{minutes}:{seconds:D2}";
    }

    // ------------------ HELPERS ------------------
    private int GetCount()
    {
        int count = 0;
        Node curr = head;
        while (curr != null)
        {
            count++;
            curr = curr.Next;
        }
        return count;
    }

    private Node GetNodeAt(int index)
    {
        Node current = head;
        for (int i = 0; i < index; i++)
            current = current.Next;
        return current;
    }
}
