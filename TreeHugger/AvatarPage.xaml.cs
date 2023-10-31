using TreeHugger.Drawable;
namespace TreeHugger;

public partial class AvatarPage : ContentPage
{
	public AvatarPage()
	{
		InitializeComponent();
	}
    private void TryOnBlueCap_Clicked(object sender, EventArgs e)
    {
        //Canvas.Drawable = (IDrawable)Resources["stickFigure"];
        StickFigure sf = (StickFigure)Resources["stickFigure"];
        if (sf.ToBeDrawn.Contains(1))
        {
            sf.ToBeDrawn.Remove(1);
        }
        else
        {

            sf.ToBeDrawn.Add(1);
        }
        Canvas.Invalidate();
        //var blueBaseballCap = (IDrawable)Resources["blueBaseballCap"];
        //var graphicsView = this.FindByName<GraphicsView>("gView");
        //if (graphicsView != null)
        //{
        //    var size = new Size(graphicsView.Width, graphicsView.Height);
        //    blueBaseballCap.Draw(graphicsView,size);
        //}
    }
    private void BuyBlueCap_Clicked(object sender, EventArgs e)
    {
        StickFigure sf = (StickFigure)Resources["stickFigure"];
        sf.AddOwnedItem(1);
    }

    private void TryOnScaredFace_Clicked(object sender, EventArgs e)
    {
        StickFigure sf = (StickFigure)Resources["stickFigure"];
        if (sf.ToBeDrawn.Contains(2))
        {
            sf.ToBeDrawn.Remove(2);
        }
        else
        {

            sf.ToBeDrawn.Add(2);
        }
        Canvas.Invalidate();
    }

    private void BuyScaredFace_Clicked(object sender, EventArgs e)
    {
        StickFigure sf = (StickFigure)Resources["stickFigure"];
        sf.AddOwnedItem(2);
    }

    private async void Shop_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ShopPage());
    }

    private void Equip_Clicked(object sender, EventArgs e)
    {

    }
}