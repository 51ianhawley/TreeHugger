using Microsoft.Maui.Controls.Maps;
using TreeHugger.Models;

namespace TreeHugger;

public partial class MapPage : ContentPage
{

    LocationServices locationServices = new();

    public MapPage()
	{
        InitializeComponent();
    }

    private async void MarkTreeButton_Clicked(object sender, EventArgs e)
    {
        Pin pin = new()
        {
            Label = "Some Tree",
            Address = "City",
            Type = PinType.Place,
            Location = new Location(await locationServices.GetCurrentLocation())
        };
        map.Pins.Add(pin);
    }

    private void ProfilePictureButton_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new ProfilePage());
    }
}
