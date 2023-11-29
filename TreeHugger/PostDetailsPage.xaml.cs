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
        float longitude = float.Parse(this.tree.Longitude);
        longLB.Text = "Longitude: "+longitude.ToString("0.000");
        float latitude = float.Parse(this.tree.Latitude);
        latLB.Text = "Latitude: "+latitude.ToString("0.000");
    }

    private void SendComment_Clicked(object sender, EventArgs e)
    {
        if (comment.Text != null)
        {
            Comment c = new Comment(comment.Text);
            MauiProgram.BusinessLogic.DataBase.AddCommentToTree(tree, c);
            comment.Placeholder = "comment was saved";
            comment.Text = null;
            this.tree = MauiProgram.BusinessLogic.DataBase.SelectTree(tree.Id);
            comments.BindingContext = this.tree;
        }
        else
        {
            comment.Placeholder = "you tried to submit an empty comment";
        }
    }
}