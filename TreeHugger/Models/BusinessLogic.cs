using System.Collections.ObjectModel;
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

    public BusinessLogic()
    {
        dataBase = new DataBase();
    }
    public TreeAdditionError AddTree(int id, int speciesId,  string location, string latitude, string longitude, Byte[] image)
    {
        
        var result = CheckTreeFields(id, location, latitude, longitude, image);
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
            Double.Parse(latitude),
            Double.Parse(longitude),
            image);
        dataBase.InsertTree(tree);
        return TreeAdditionError.NoError;
        
        
        


    }
    public TreeAdditionError CheckTreeFields(int id, String location, String latitude, String longitude, Byte[] image)
    {
        if (dataBase.SelectTree(id) != null)
        {
            return TreeAdditionError.TreeAlreadyExists;
        }
        if (string.IsNullOrEmpty(location))
        {
            return TreeAdditionError.EmptyLocation;
        }
        var latitudeTest = 0.00;
        if (!double.TryParse(latitude, out latitudeTest))
        {
            return TreeAdditionError.InvalidLatitudeValue;
        }
        var longitudeTest = 0.00;
        if (!double.TryParse(longitude, out longitudeTest))
        {
            return TreeAdditionError.InvalidLatitudeValue;
        }
        if (image.Length > 0)
        {
            return TreeAdditionError.NoTreeImagePresent;
        }
        return TreeAdditionError.NoError;

    }


}
