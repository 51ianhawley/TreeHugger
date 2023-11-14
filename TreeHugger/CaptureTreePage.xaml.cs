namespace TreeHugger;

public partial class CaptureTreePage : ContentPage
{
	public CaptureTreePage()
	{
		InitializeComponent();
	}

    private async void cameraView_CamerasLoaded(object sender, EventArgs e)
    {
		cameraView.Camera = cameraView.Cameras[0];
		await cameraView.StartCameraAsync().ConfigureAwait(false);
    }
}