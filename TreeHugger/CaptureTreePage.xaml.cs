

namespace TreeHugger;

public partial class CaptureTreePage : ContentPage
{
	public CaptureTreePage()
	{
		InitializeComponent();
        var items = new List<string> { "Birch", "Oak", "Pine" };
        pckSpeices.ItemsSource = items;
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
                Console.WriteLine("A picture has been successfully takne.");
                Console.WriteLine(localFileStream.ToString());
                
                imgTree.Source = ImageSource.FromFile(localFilePath);
                //await sourceStream.CopyToAsync(localFileStream);
            }
        }
    }

    private void btnAddTree_Clicked(object sender, EventArgs e)
    {

    }
}