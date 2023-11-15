#if IOS || ANDROID || MACCATALYST
using Microsoft.Maui.Graphics.Platform;
using Microsoft.Maui.Platform;
using System;
using System.Collections.ObjectModel;
#elif WINDOWS
using Microsoft.Maui.Graphics.Win2D;
#endif
using System.Reflection;
using System.Text.Json;
using TreeHugger.Models;
using IImage = Microsoft.Maui.Graphics.IImage;

namespace TreeHugger.Drawable;

public class StickFigure : IDrawable
{
    string fullPathOwnedItems;
    string fullPathEquippedItems;
    string fullPathItems;
    String FILE_NAME_ITEMS = "Items.db.txt";
    String FILE_NAME_OWNED_ITEMS = "ownedItems.db.txt";
    String FILE_NAME_EQUIPPED_ITEMS = "equippedItems.db.txt";
    List<int> toBeDrawn = new List<int>();
    List<int> ownedItems = new List<int>();
    List<int> equippedItems = new List<int>();
    List<Item> items = new List<Item>();
    public List<int> ToBeDrawn
    {
        get { return toBeDrawn; }
        set { toBeDrawn = value; }
    }
    public List<int> OwnedItems{ 
        get { return ownedItems; } 
        set { ownedItems = value; }
    }
    public List<int> EquippedItems { 
        get { return equippedItems; }
        set { equippedItems = value; }
    }
    public List<Item> Items { 
        get { return items; }
        set { Items = value; }
    }

    

    public StickFigure()
    {
        fullPathOwnedItems = System.IO.Path.Combine(FileSystem.Current.AppDataDirectory, FILE_NAME_OWNED_ITEMS);
        fullPathEquippedItems = System.IO.Path.Combine(FileSystem.Current.AppDataDirectory, FILE_NAME_EQUIPPED_ITEMS);
        fullPathItems = System.IO.Path.Combine(FileSystem.Current.AppDataDirectory, FILE_NAME_ITEMS);

        string jsonOwnedItems;
        if (!File.Exists(fullPathOwnedItems))
        {
            jsonOwnedItems = JsonSerializer.Serialize(OwnedItems);
            File.WriteAllText(fullPathOwnedItems, jsonOwnedItems);
        }
        jsonOwnedItems = File.ReadAllText(fullPathOwnedItems);
        OwnedItems = JsonSerializer.Deserialize<List<int>>(jsonOwnedItems);
        string jsonEquippedItems;
        if (!File.Exists(fullPathEquippedItems))
        {
            jsonEquippedItems = JsonSerializer.Serialize(EquippedItems);
            File.WriteAllText(fullPathEquippedItems, jsonEquippedItems);
        }
        jsonEquippedItems = File.ReadAllText(fullPathEquippedItems);
        EquippedItems = JsonSerializer.Deserialize<List<int>>(jsonEquippedItems);
        string jsonItems;
        if (!File.Exists(fullPathItems))
        {
            jsonItems = JsonSerializer.Serialize(Items);
            File.WriteAllText(fullPathItems, jsonItems);
        }
        jsonItems = File.ReadAllText(fullPathItems);
        //Items = JsonSerializer.Deserialize<List<Item>>(jsonItems);
        if (!EquippedItems.Contains(0))
        {
            EquippedItems.Add(0);
        }
        foreach (int index in EquippedItems)
        {
            if (!ToBeDrawn.Contains(index))
            {
                ToBeDrawn.Add(index);
            }
        }
        List<int> offset = new List<int>(2);
        List<int> size = new List<int>(2);
        offset.Add(10);
        offset.Add(10);
        size.Add(85);
        size.Add(200);
        Items.Add(new Item(0,"stick_figure", 0, offset[0], offset[1], size[0], size[1],new byte[1]));
        offset[0] = 10;
        offset[1] = -20;
        size[0] = 80;
        size[1] = 80;
        Items.Add(new Item(1,"blue_baseball_cap", 0, offset[0], offset[1], size[0], size[1], new byte[1]));
        offset[0] = 40;
        offset[1] = 35;
        size[0] = 25;
        size[1] = 25;
        Items.Add(new Item(2,"scared_face_hi", 0, offset[0], offset[1], size[0], size[1], new byte[1]));
    }
    public void AddOwnedItem(int index)
    {
        OwnedItems.Add(index);
        string jsonOwnedItems = JsonSerializer.Serialize(OwnedItems);
        File.WriteAllText(fullPathOwnedItems, jsonOwnedItems);
    }
    public void EquipItems()
    {
        EquippedItems.Clear();
        EquippedItems = new List<int>(ToBeDrawn);
        string jsonEquippedItems = JsonSerializer.Serialize(EquippedItems);
        File.WriteAllText(fullPathEquippedItems, jsonEquippedItems);
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
                IImage newImage = image.Downsize(items[index].XSize, items[index].YSize, true);
                canvas.DrawImage(newImage, items[index].XOffset, items[index].YOffset, newImage.Width, newImage.Height);
            }
        }
    }
}
