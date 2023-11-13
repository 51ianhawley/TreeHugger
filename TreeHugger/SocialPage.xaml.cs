using CommunityToolkit.Maui.Converters;
using Microsoft.Maui.Controls;
using TreeHugger.Models;

namespace TreeHugger;

public partial class SocialPage : ContentPage
{
	public SocialPage()
	{
		InitializeComponent();
        BindingContext = MauiProgram.BusinessLogic;
    }

    private async void Tree_Clicked(object sender, EventArgs e)
    {
        Tree tree = CV.SelectedItem as Tree;
        await Navigation.PushAsync(new PostDetailsPage(tree));
    }
}
