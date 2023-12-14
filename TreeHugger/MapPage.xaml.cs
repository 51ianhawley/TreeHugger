using System.Collections.ObjectModel;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using TreeHugger.Models;

namespace TreeHugger;

public partial class MapPage : ContentPage
{
    private readonly ObservableCollection<Pin> pins = MauiProgram.BusinessLogic.Pins;
    private readonly Task<Location> location = LocationServices.GetCurrentLocation();

    public MapPage()
	{
        InitializeComponent();
        MoveMapToCurrentLocation(map, location, 0.01, 0.01);
        PopulateMapWithPins(map, pins);
    }

    /// <summary>
    /// Populates map with pins
    /// </summary>
    /// <param name="map">current map</param>
    /// <param name="pins">pins to populate</param>
    private static void PopulateMapWithPins(Microsoft.Maui.Controls.Maps.Map map, ObservableCollection<Pin> pins)
    {
        foreach (Pin pin in pins)
        {
            map.Pins.Add(pin);
        }
    }

    /// <summary>
    /// Move mapspan to current location
    /// </summary>
    /// <param name="map">current map</param>
    /// <param name="location">current location</param>
    /// <param name="latitudeDegrees">width</param>
    /// <param name="longitudeDegrees">height</param>
    private static async void MoveMapToCurrentLocation(Microsoft.Maui.Controls.Maps.Map map, Task<Location> location, double latitudeDegrees, double longitudeDegrees)
    {
        map.MoveToRegion(new MapSpan(await location, latitudeDegrees, longitudeDegrees));
    }

    private async void MarkTreeButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CaptureTreePage());
    }
}
