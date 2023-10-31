using TreeHugger.Drawable;

namespace TreeHugger;

public partial class Shop : ContentPage
{
	public Shop()
	{
		InitializeComponent();
	}

    private void OnAddImageClicked(object sender, EventArgs e)
    {
        //Canvas.Drawable = (IDrawable)Resources["stickFigure"];
        StickFigure sf = (StickFigure)Resources["stickFigure"];
        sf.ToBeDrawn.Add(1);
        Canvas.Invalidate();
        //var blueBaseballCap = (IDrawable)Resources["blueBaseballCap"];
        //var graphicsView = this.FindByName<GraphicsView>("gView");
        //if (graphicsView != null)
        //{
        //    var size = new Size(graphicsView.Width, graphicsView.Height);
        //    blueBaseballCap.Draw(graphicsView,size);
        //}
    }

    private void BuyScaredFace_Clicked(object sender, EventArgs e)
    {
        StickFigure sf = (StickFigure)Resources["stickFigure"];
        sf.ToBeDrawn.Add(2);
        Canvas.Invalidate();
    }
}