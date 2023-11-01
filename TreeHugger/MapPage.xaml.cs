namespace TreeHugger;

public partial class MapPage : ContentPage
{
	public MapPage()
	{
        InitializeComponent();
    }

    private void MarkTreeButton_Clicked(object sender, EventArgs e)
    {
        // Perform some action when the button is clicked
        // For example, display a message
        DisplayAlert("Button Clicked", "You clicked the button!", "OK");
    }

    private void ProfilePictureButton_Clicked(object sender, EventArgs e)
    {
        // Perform some action when the button is clicked
        // For example, display a message
        Navigation.PushAsync(new ProfilePage());
    }
}
