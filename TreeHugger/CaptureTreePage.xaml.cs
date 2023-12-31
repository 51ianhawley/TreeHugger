

using System.Collections.ObjectModel;
using TreeHugger.Models;

namespace TreeHugger;

public partial class CaptureTreePage : ContentPage
{
    private string _savedImagePath;
    
	public CaptureTreePage()
	{
		InitializeComponent();
        //var items = new List<string> { "Birch", "Oak", "Pine" };
        //pckSpeices.ItemsSource = items;
        var obsrvColSpecies = MauiProgram.BusinessLogic.Species;
        var pckItemsToAdd = new List<string>();
        foreach (var species in obsrvColSpecies)
        {
            pckItemsToAdd.Add(species.Name);
        }
        pckSpeices.ItemsSource = pckItemsToAdd;
        SetLocationLabels();
    }
    /// <summary>
    /// sets labels to show latitude and longitude on the CaptureTreePage
    /// </summary>
    private async void SetLocationLabels()
    {
        Location location = await LocationServices.GetCurrentLocation();
        lblLongitudeOutput.Text = location.Longitude.ToString().Substring(0,6);
        lblLatitudeOutput.Text = location.Latitude.ToString().Substring(0, 6);
    }

	private async void btnTakePhoto_Clicked(object sender, EventArgs e)
	{
        if (MediaPicker.Default.IsCaptureSupported)
        {
            FileResult photo = await MediaPicker.Default.CapturePhotoAsync();

            if (photo != null)
            {
                // save the file into local storage
                
                string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

                using Stream sourceStream = await photo.OpenReadAsync();
                using FileStream localFileStream = File.OpenWrite(localFilePath);
                await sourceStream.CopyToAsync(localFileStream);
                Console.WriteLine("A picture has been successfully taken.");
                Console.WriteLine(localFileStream.ToString());
                _savedImagePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
                imgTree.Source = ImageSource.FromFile(localFilePath);
                
                //await sourceStream.CopyToAsync(localFileStream);
            }
        }
    }

    private async void btnAddTree_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(_savedImagePath))
            {
                var newTreeId = MauiProgram.BusinessLogic.GetMaxTreeId();
                var comments = new ObservableCollection<Comment>();
                // Take the image and convert it into bytes to be inserted into the database.
                byte[] imgBytes;
                using (FileStream fileStream = new FileStream(_savedImagePath, FileMode.Open, FileAccess.Read))
                {
                    imgBytes = new byte[fileStream.Length];
                    await fileStream.ReadAsync(imgBytes, 0, imgBytes.Length);

                }
                // Code is having an issue that will be resolved by the presentation involving an invocation exception that is being thrown in the Post details page.
                Location location = await LocationServices.GetCurrentLocation();
                var newTree = new Tree(newTreeId, pckSpeices.SelectedIndex, NameENT.Text, location.Latitude.ToString(), location.Longitude.ToString(), imgBytes);
                var result = MauiProgram.BusinessLogic.InsertTree(newTree);
                //var result = MauiProgram.BusinessLogic.AddTree(newTreeId,
                //    pckSpeices.SelectedIndex,
                //    "Out there",
                //    lblLatitudeOutput.Text,
                //    lblLongitudeOutput.Text,
                //    imgBytes,
                //    comments);
                await Navigation.PushAsync(new MainTabbedPage());
                
                if (result != true)
                {
                    Console.WriteLine("OPE The tree was not added correctly: " + result.ToString());
                    await Navigation.PushAsync(new MainTabbedPage()); // Go back to the main page if something went wrong.

                }
                Console.WriteLine("got here");
                //await Navigation.PushAsync(new PostDetailsPage(newTree));

            }
            else
            {
                Console.WriteLine("The image was not written to the local cache directory.");

            }
            //await Navigation.PushAsync(new MainTabbedPage()); // Go back to the main page if something went wrong.
        }
        catch (Exception ex)
        {

            Console.WriteLine("OPE Something really bad happened:" + ex.Message);
        }
    }
}