using TreeHugger.Models;

namespace TreeHugger;

public partial class PostDetailsPage : ContentPage
{
	public PostDetailsPage(Tree tree)
	{
		InitializeComponent();
		BindingContext = tree;
	}
}