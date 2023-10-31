#if IOS || ANDROID || MACCATALYST
using Microsoft.Maui.Graphics.Platform;
using Microsoft.Maui.Platform;
#elif WINDOWS
using Microsoft.Maui.Graphics.Win2D;
#endif
using System.Reflection;
using IImage = Microsoft.Maui.Graphics.IImage;

namespace TreeHugger.Drawable;

public class StickFigure : IDrawable
{
    List<int> toBeDrawn = new List<int>();
    List<Item> ownedItems = new List<Item>();
    public List<int> ToBeDrawn
    {
        get { return toBeDrawn; }
        set { toBeDrawn = value; }
    }
    public List<Item> OwnedItems{ get { return ownedItems; } }
    List<Item> items = new List<Item>();
    

    public StickFigure()
    {
        ToBeDrawn.Add(0);
        List<int> offset = new List<int>(2);
        List<int> size = new List<int>(2);
        offset.Add(10);
        offset.Add(10);
        size.Add(85);
        size.Add(200);
        items.Add(new Item("stick_figure", 0, new List<int>(offset), new List<int>(size)));
        offset[0] = 10;
        offset[1] = -20;
        size[0] = 80;
        size[1] = 80;
        items.Add(new Item("blue_baseball_cap", 0, new List<int>(offset), new List<int>(size)));
        offset[0] = 40;
        offset[1] = 35;
        size[0] = 25;
        size[1] = 25;
        items.Add(new Item("scared_face_hi", 0, new List<int>(offset), new List<int>(size)));
    }
    public void AddOwnedItem(int index)
    {
        OwnedItems.Add(items[index]);
    }
    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        
        foreach(var index in toBeDrawn) 
        {
            IImage image;
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            using (Stream stream = assembly.GetManifestResourceStream("TreeHugger.Resources.Images." + items[index].Name +".png"))
        {
#if IOS || ANDROID || MACCATALYST
            // PlatformImage isn't currently supported on Windows.
            image = PlatformImage.FromStream(stream);
#elif WINDOWS
    image = new W2DImageLoadingService().FromStream(stream);
#endif
        }

            if (image != null)
            {
                IImage newImage = image.Downsize(items[index].Size[0], items[index].Size[1], true);
                canvas.DrawImage(newImage, items[index].Offset[0], items[index].Offset[1], newImage.Width, newImage.Height);
            }
        }
    }
}
