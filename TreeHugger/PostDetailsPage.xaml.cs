using TreeHugger.Models;

namespace TreeHugger;

public partial class PostDetailsPage : ContentPage
{
	Tree tree;
	public PostDetailsPage(Tree tree)
	{
		this.tree = tree;
		InitializeComponent();
		BindingContext = tree;
	}
}