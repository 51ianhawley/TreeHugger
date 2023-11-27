using TreeHugger.Models;

namespace TreeHugger;

public partial class PostDetailsPage : ContentPage
{
	public PostDetailsPage(Tree tree)
	{
		InitializeComponent();
		details.BindingContext = tree;
		comments.BindingContext = tree.Comments;
	}
}