﻿using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using TreeHugger.Models;

namespace TreeHugger;

public partial class MapPage : ContentPage
{

    public MapPage()
	{
        InitializeComponent();
        SetMapSpanOnTimer(1);
    }

    private async void SetMapSpan()
    {
        map.MoveToRegion(new MapSpan(await LocationServices.GetCurrentLocation(), 0.01, 0.01));
    }

    private void SetMapSpanOnTimer(int seconds)
    {
        var timer = Application.Current.Dispatcher.CreateTimer();
        timer.Interval = TimeSpan.FromSeconds(seconds);
        timer.Tick += (s, e) => SetMapSpan();
        timer.Start();
    }

    private async void MarkTreeButton_Clicked(object sender, EventArgs e)
    {
        Pin pin = new()
        {
            Label = "Some Tree",
            Address = "City",
            Type = PinType.Place,
            Location = new Location(await LocationServices.GetCurrentLocation())
        };
        map.Pins.Add(pin);
        await Navigation.PushAsync(new CaptureTreePage());
        //TakePhoto();
    }
    /// <summary>
    /// no longer in use
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
    /// <summary>
    /// navigates to profile page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Canvas_StartInteraction(object sender, TouchEventArgs e)
    {
        Navigation.PushAsync(new ProfilePage());
    }
}
