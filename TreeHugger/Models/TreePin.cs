using Npgsql.Internal.TypeHandlers.LTreeHandlers;
using System;
using System.ComponentModel;

namespace TreeHugger.Models;

[Serializable()]
public class TreePin : INotifyPropertyChanged
{
    int id;
    String name;
    Location location;

    public int Id
    {
        get { return id; }
        set
        {
            id = value;
            OnPropertyChanged(nameof(Id));
        }
    }

    public String Name
    {
        get { return name; }
        set
        {
            name = value;
            OnPropertyChanged(nameof(Species));
        }
    }
    public Location Location
    {
        get { return location; }
        set
        {
            location = value;
            OnPropertyChanged(nameof(Location));
        }
    }

    public TreePin(int id, String species, double latitude, double longitude)
    {
        Id = id;
        Name = species;
        Location = new Location(latitude, longitude);
    }

    public TreePin() { }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public override bool Equals(object obj)
    {
        var otherTreePin = obj as TreePin;
        return Id == otherTreePin.Id;
    }

    public override int GetHashCode()
    {
        throw new NotImplementedException();
    }
}

