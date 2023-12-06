using System.Reflection;
using System.Text.Json;
using TreeHugger.Drawable;
namespace TreeHugger;

public partial class AvatarPage : ContentPage
{
	public AvatarPage()
	{
        BindingContext = MauiProgram.BusinessLogic;
        InitializeComponent();
        //username changing
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

        StickFigure sf = (StickFigure)Resources["stickFigure"];
        Image image;
        if (!sf.OwnedItems.Contains(0))
        {
            sf.OwnedItems.Add(0);
        }
        foreach (int index in sf.OwnedItems)
        {

            Assembly assembly = GetType().GetTypeInfo().Assembly;
            using (Stream stream = assembly.GetManifestResourceStream("TreeHugger.Resources.Images." + sf.Items[index].Name + ".png"))
            {
#if IOS || ANDROID || MACCATALYST
                // PlatformImage isn't currently supported on Windows.
                image = new Image { Source = ImageSource.FromStream(() => stream) };
#elif WINDOWS
    //image = new W2DImageLoadingService().FromStream(stream);
#endif
            }
            image = new Image { Source = "TreeHugger.Resources.Images." + sf.Items[index].Name + ".png" };
            if (image != null)
            {
                _Grid.Children.Add(image);
                _Grid.SetRow(image, 2);
                _Grid.SetColumn(image, 1);
            }
        }

    }
    private void TryOnBlueCap_Clicked(object sender, EventArgs e)
    {
        Canvas.Drawable = (IDrawable)Resources["stickFigure"];
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
        StickFigure sf = (StickFigure)Resources["stickFigure"];
        sf.EquipItems();
        //sf.AddEquippedItem(sf.ToBeDrawn[sf.ToBeDrawn.Count-1]);
    }

    private void Buy_Clicked(object sender, EventArgs e)
    {
        return;
    }

    private void usernameEntry_Completed(object sender, EventArgs e)
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