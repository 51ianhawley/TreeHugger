namespace TreeHugger;

public class Item
{
    public string Name { get; set; }
    public int Cost { get; set; }
    public int XOffset { get; set; }
    public int YOffset { get; set; }
    public int XSize { get; set; }
    public int YSize { get; set; }
    public List<int> Size { get; set; }
    public Byte[] Image { get; set; }
    public Item(string name, int cost, int xOffset, int yOffset, int xSize,int ySize, Byte[] image)
    {
        Name = name;
        Cost = cost;
        XOffset = xOffset;
        YOffset = yOffset;
        XSize = xSize;
        YSize = ySize;
    }
}
