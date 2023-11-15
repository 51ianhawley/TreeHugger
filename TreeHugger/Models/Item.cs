using System.ComponentModel;

namespace TreeHugger.Models;

public class Item : INotifyPropertyChanged
{
    int id;
    string name;
    int cost;
    int xOffset;
    int yOffset;
    int xSize;
    int ySize;
    byte[] image;
    public int Id
    {
        get { return id; }
        set
        {
            id = value;
            OnPropertyChanged(nameof(id));
        }
    }
    public string Name
    {
        get { return name; }
        set
        {
            name = value;
            OnPropertyChanged(nameof(name));
        }
    }
    public int Cost
    {
        get { return cost; }
        set
        {
            cost = value;
            OnPropertyChanged(nameof(cost));
        }
    }
    public int XOffset
    {
        get { return xOffset; }
        set
        {
            xOffset = value;
            OnPropertyChanged(nameof(xOffset));
        }
    }
    public int YOffset
    {
        get { return yOffset; }
        set
        {
            yOffset = value;
            OnPropertyChanged(nameof(yOffset));
        }
    }
    public int XSize
    {
        get { return xSize; }
        set
        {
            xSize = value;
            OnPropertyChanged(nameof(xSize));
        }
    }
    public int YSize
    {
        get { return ySize; }
        set
        {
            ySize = value;
            OnPropertyChanged(nameof(ySize));
        }
    }
    public byte[] Image
    {
        get { return image; }
        set
        {
            image = value;
            OnPropertyChanged(nameof(image));
        }
    }
    public Item()
    {
        return;
    }
    public Item(int id, string name, int cost, int xOffset, int yOffset, int xSize, int ySize, byte[] image)
    {
        Id = id;
        Name = name;
        Cost = cost;
        XOffset = xOffset;
        YOffset = yOffset;
        XSize = xSize;
        YSize = ySize;
    }
    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
