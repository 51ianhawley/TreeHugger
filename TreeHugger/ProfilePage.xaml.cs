namespace TreeHugger;

public partial class ProfilePage : ContentPage
{
	public ProfilePage()
	{
		InitializeComponent();
	}

    private void OnSaveClicked(object sender, EventArgs e)
    {
        // Implement your save logic here, e.g., save the settings to a data store.
        string fullName = FullNameEntry.Text;
        string emailAddress = EmailEntry.Text;
        bool pushNotifications = PushNotificationSwitch.IsToggled;
        bool emailNotifications = EmailNotificationSwitch.IsToggled;

        ProfileNameLabel.Text = FullNameEntry.Text;

        // You can save the settings to a database, preferences, or wherever is suitable for your app.
    }
}
