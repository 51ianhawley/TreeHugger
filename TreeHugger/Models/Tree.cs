
using Microsoft.Maui.Devices.Sensors;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.Json;
using System.Windows.Input;

namespace TreeHugger.Models;

public class Tree : INotifyPropertyChanged 
{
    int _Id;
    int _SpeciesId;
    string _Location;
    string _Latitude;
    string _Longitude;
    Byte[] _Image;
    public Tree() 
    {
        this.Id = -1;
        this._SpeciesId = -1;
        this._Location = null;
        this._Latitude = null;
        this._Longitude = null;
        this._Image = null;
    }
    public Tree(int id, int speciesId, string location, string latitude, string longitude, Byte[] image)
    {
        this.Id = id;
        this._SpeciesId = speciesId;
        this._Location = location;
        this._Latitude = latitude;
        this._Longitude = longitude;
        this._Image = image;
        NavigateToDetailsCommand = new Command(NavigateToDetails);
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
    public string Latitude
    {
        get { return _Latitude; }
        set
        {
            _Latitude = value;
            OnPropertyChanged(nameof(_Location));
        }
    }
    public string Longitude
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
    public ObservableCollection<Comment> Comments { 
        get { return MauiProgram.BusinessLogic.GetComments(this.Id); }
    }
    public ICommand NavigateToDetailsCommand { get; }
    public String GetComments()
    {
        String jsonComments = "failed converstion to json";
        jsonComments = JsonSerializer.Serialize(Comments);
        return jsonComments;
    }
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void Delete()
    {
        MauiProgram.BusinessLogic.DeleteTree(this);
    }
    public async void NavigateToDetails()
    {
        await  App.Current.MainPage.Navigation.PushAsync(new PostDetailsPage(this));
    }
}




