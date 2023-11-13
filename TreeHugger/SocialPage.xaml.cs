using CommunityToolkit.Maui.Converters;

namespace TreeHugger;

public partial class SocialPage : ContentPage
{
	public SocialPage()
	{
		InitializeComponent();
        BindingContext = MauiProgram.BusinessLogic;
        //var image = new Image();
        //byte[] b = MauiProgram.BusinessLogic.DataBase.SelectTree(999).Image;
        //Image image = TestImage;
        //image.SetBinding(
        //    Image.SourceProperty,
        //    new Binding(
        //        nameof(b),
        //        mode: BindingMode.OneWay,
        //        converter: new ByteArrayToImageSourceConverter()));


    }
}
