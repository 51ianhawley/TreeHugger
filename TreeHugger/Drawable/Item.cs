namespace TreeHugger;

public class Item
{
    public string Name { get; set; }
    public int Cost { get; set; }
    public List<int> Offset { get; set; }
    public List<int> Size { get; set; }
    public Item(string name, int cost, List<int> offset, List<int> size)
    {
        Name = name;
        Cost = cost;
        Offset = offset;
        Size = size;
    }
}
