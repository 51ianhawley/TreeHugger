
using System.ComponentModel;

namespace TreeHugger.Models;

public class Tree : INotifyPropertyChanged
{
    int _Id;
    int _SpeciesId;
    string _Location;
    string _Lattiude;
    string _Longitude;
    Byte[] _Image;

    public Tree(int id, int speciesID, string location, string latitude, string longitude, Byte[] image)
    {
        this.Id = id;
        this._SpeciesId = speciesID;
        this._Location = location;
        this._Lattiude = latitude;
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
    public string Lattiude
    {
        get { return _Lattiude; }
        set
        {
            _Location = value;
            OnPropertyChanged(nameof(_Location));
        }
    }
    public string Longitude
    {
        get { return _Longitude; }
        set
        {
            _Location = value;
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




