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
        await Navigation.PushAsync(new CaptureTreePage());
        //TakePhoto();
    }

    private void ProfilePictureButton_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new ProfilePage());
    }
    public async void TakePhoto()
    {
        if(MediaPicker.Default.IsCaptureSupported)
        {
            FileResult photo = await MediaPicker.Default.CapturePhotoAsync();
            if(photo != null)
            {
                string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
                using Stream sourceStream = await photo.OpenReadAsync();
                using FileStream localFileStream =  File.OpenWrite(localFilePath);
                await sourceStream.CopyToAsync(localFileStream);
                Console.WriteLine("I captured a photo Yipee!!????");
            }

        }
    }
}
