
using Microsoft.Maui.Devices.Sensors;
using System.ComponentModel;

namespace TreeHugger.Models;

public class Tree : INotifyPropertyChanged
{
    int _Id;
    int _SpeciesId;
    string _Location;
    double _Latitude;
    double _Longitude;
    Byte[] _Image;
    public Tree() 
    {
        this.Id = -1;
        this._SpeciesId = -1;
        this._Location = null;
        this._Latitude = 0;
        this._Longitude = 0;
        this._Image = null;
    }
    public Tree(int id, int speciesId, string location, double latitude, double longitude, Byte[] image)
    {
        this.Id = id;
        this._SpeciesId = speciesId;
        this._Location = location;
        this._Latitude = latitude;
        this._Longitude = longitude;
        this._Image = image;
    }
    public int Id
    {
        get { return _Id; }
        set
        {
            _Id = value;
            OnPropertyChanged(nameof(_Id));
        }
    }
    public int SpeciesId
    {
        get { return _SpeciesId; }
        set
        {
            _SpeciesId = value;
            OnPropertyChanged(nameof(_SpeciesId));
        }
    }
    public string Location
    {
        get { return _Location; }
        set
        {
            _Location = value;
            OnPropertyChanged(nameof(_Location));
        }
    }
    public double Latitude
    {
        get { return _Latitude; }
        set
        {
            _Latitude = value;
            OnPropertyChanged(nameof(_Location));
        }
    }
    public double Longitude
    {
        get { return _Longitude; }
        set
        {
            _Longitude = value;
            OnPropertyChanged(nameof(_Longitude));
        }
    }
    public byte[] Image
    {
        get { return _Image; }
        set
        {
            _Image = value;
            OnPropertyChanged(nameof(_Image));
        }
    }
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}




