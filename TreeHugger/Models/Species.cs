using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace TreeHugger.Models;

public class Species : INotifyPropertyChanged
{
    int _Id;
    string _Name;
    string _LocationsFound;
    string _Color;
    Byte[] _ExampleImage;

    public Species()
    {
        this._Id = 0;
        this._Name = string.Empty;
        this._LocationsFound = string.Empty;
        this._Color = string.Empty;
        this._ExampleImage = null;
    }
    public Species(int id, string name, string locationsFound, string color, Byte[] exampleImage)
    {
        this._Id = id;
        this._Name = name;
        this._LocationsFound = locationsFound;
        this._Color = color;
        this._ExampleImage = exampleImage;
    }
    public int Id
    {
        get { return this._Id; }
        set 
        { 
            _Id = value;
            OnPropertyChanged(nameof(_Id));
        }
    }
    public string Name
    {
        get { return this._Name; }
        set
        {
            this._Name = value;
            OnPropertyChanged(nameof(_Name));
        }
    }
    public string Color
    {
        get { return this._Color; }
        set
        {
            this._Color = value;
            OnPropertyChanged(nameof(_Color));
        }
    }
    public byte[] ExampleImage
    {
        get { return _ExampleImage; }
        set
        {
            _ExampleImage = value;
            OnPropertyChanged(nameof(_ExampleImage));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
