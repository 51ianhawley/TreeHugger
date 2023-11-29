using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using static TreeHugger.Models.ErrorReporting;

namespace TreeHugger.Models;

public class BusinessLogic
{
    DataBase dataBase;
    public ObservableCollection<Tree> Trees 
    {
        get { return dataBase.SelectAllTrees(); }
    }
    public ObservableCollection<Item> Items
    {
        get { return dataBase.SelectAllItems(); }
    }

    public DataBase DataBase { get { return dataBase; } }
    public String Username { get; set; }

    public BusinessLogic()
    {
        dataBase = new DataBase();
        Username = "Guest";
    }

    public TreeAdditionError AddTree(int id, int speciesId, string location, string latitude, string longitude, Byte[] image, ObservableCollection<Comment> comments)
    {
        
        var result = CheckTreeFields(id, location, latitude, longitude, image);
        var serializedComments = JsonSerializer.Serialize(comments);
        if (result != TreeAdditionError.NoError)
        {
            return result;
        }
        if (dataBase.SelectTree(id) != null)
        {
            return TreeAdditionError.DuplicateTreeError;
        }
        
        Tree tree = new Tree(id,
            speciesId, location,
            latitude,
            longitude,
            image
            );
        dataBase.InsertTree(tree);
        return TreeAdditionError.NoError;
        
        
        


    }
    public TreeAdditionError CheckTreeFields(int id, String location, String latitude, String longitude, Byte[] image)
    {
        if (dataBase.SelectTree(id) != null)
        {
            return TreeAdditionError.TreeAlreadyExists;
        }
        //var latitudeTest = 0.00;
        //if (!double.TryParse(latitude, out latitudeTest))
        //{
        //    return TreeAdditionError.InvalidLatitudeValue;
        //}
        //var longitudeTest = 0.00;
        //if (!double.TryParse(longitude, out longitudeTest))
        //{
        //    return TreeAdditionError.InvalidLatitudeValue;
        //}
        if (image.Length <= 0)
        {
            return TreeAdditionError.NoTreeImagePresent;
        }
        return TreeAdditionError.NoError;

    }


}
