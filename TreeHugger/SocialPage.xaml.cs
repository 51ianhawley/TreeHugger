using CommunityToolkit.Maui.Converters;
using Microsoft.Maui.Controls;
using TreeHugger.Models;

namespace TreeHugger;

public partial class SocialPage : ContentPage
{
	public SocialPage()
	{
        BindingContext = MauiProgram.BusinessLogic;
        InitializeComponent();
        
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
}
