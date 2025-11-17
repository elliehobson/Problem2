public class Node
{
    public Song Data { get; set; }
    public Node Next { get; set; }

    public Node(Song data)
    {
        Data = data;
        Next = null;
    }
}
