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

    private void SendComment_Clicked(object sender, EventArgs e)
    {
        if (comment.Text != null)
        {
            Comment c = new Comment(comment.Text);
            tree._Comments.Add(c);
            MauiProgram.BusinessLogic.DataBase.UpdateTree(tree, tree.SpeciesId, tree.Location, tree.Lattiude, tree.Longitude, tree.Image);
            comment.Placeholder = "comment was saved";
            comment.Text = null;
        }
    }
}