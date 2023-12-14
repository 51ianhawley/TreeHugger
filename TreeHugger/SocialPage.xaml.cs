using CommunityToolkit.Maui.Converters;
using Microsoft.Maui.Controls;
using System.Text.Json;
using TreeHugger.Models;

namespace TreeHugger;

public partial class SocialPage : ContentPage
{
	public SocialPage()
	{
        BindingContext = MauiProgram.BusinessLogic;
        InitializeComponent();
        SetUsername();

    }
    /// <summary>
    /// sets the username in the entry
    /// </summary>
    private async void SetUsername()
    {
        string fullPathUsername = System.IO.Path.Combine(FileSystem.Current.AppDataDirectory, "username");

        string jsonUsername;
        if (!File.Exists(fullPathUsername))
        {
            jsonUsername = JsonSerializer.Serialize(MauiProgram.BusinessLogic.Username);
            File.WriteAllText(fullPathUsername, jsonUsername);
        }
        jsonUsername = File.ReadAllText(fullPathUsername);
        MauiProgram.BusinessLogic.Username = JsonSerializer.Deserialize<string>(jsonUsername);
        usernameEntry.Text = MauiProgram.BusinessLogic.Username;
    }
    /// <summary>
    /// navigates to PostDetailsPage with the selected tree
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void Tree_Clicked(object sender, EventArgs e)
    {
        Tree tree = CV.SelectedItem as Tree;
        await Navigation.PushAsync(new PostDetailsPage(tree));
    }
    private void UsernameEntry_Completed(object sender, EventArgs e)
    {
        MauiProgram.BusinessLogic.Username = usernameEntry.Text;
        string jsonUsername = JsonSerializer.Serialize(MauiProgram.BusinessLogic.Username);
        string fullPathUsername = System.IO.Path.Combine(FileSystem.Current.AppDataDirectory, "username");
        File.WriteAllText(fullPathUsername, jsonUsername);
        usernameEntry.Text = String.Empty;
        usernameEntry.Placeholder = "Your username has been saved as " + MauiProgram.BusinessLogic.Username;
        usernameLabel.IsVisible = false;
    }
}
