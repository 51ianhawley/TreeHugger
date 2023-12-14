using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Maui.Controls.Maps;
using static TreeHugger.Models.ErrorReporting;

namespace TreeHugger.Models;

public class BusinessLogic
{
    DataBase dataBase;
    public ObservableCollection<Tree> Trees 
    {
        get { return dataBase.SelectAllTrees(); }
    }
    public ObservableCollection<Pin> Pins
    {
        get { return dataBase.GenerateAllTeePins(); }
    }
    public ObservableCollection<Species> Species
    {
        get { return dataBase.SelectAllSpecies(); }
    }
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
    /// <summary>
    /// deletes tree from database
    /// </summary>
    /// <param name="tree"tree to be deleted</param>
    /// <returns>returns true if successful</returns>
    public Boolean DeleteTree(Tree tree)
    {
        return dataBase.DeleteTree(tree);
    }
    /// <summary>
    /// Gets an observableCollection of comments for a tree
    /// </summary>
    /// <param name="id">id for tree</param>
    /// <returns>observableCollection of comments</returns>
    public ObservableCollection<Comment> GetComments(int id)
    {
        return dataBase.GetComments(id);
    }
    /// <summary>
    /// Gets id for new tree
    /// </summary>
    /// <returns>returns id for new tree</returns>
    public int GetMaxTreeId()
    {
        return dataBase.GetMaxTreeId();
    }
    /// <summary>
    /// inserts a tree into the database
    /// </summary>
    /// <param name="newTree">tree to be inserted</param>
    /// <returns>boolean true if successful</returns>
    public Boolean InsertTree(Tree newTree)
    {
        return dataBase.InsertTree(newTree); 
    }
    /// <summary>
    /// Selects a tree from the database
    /// </summary>
    /// <param name="Id">id of tree to be returned</param>
    /// <returns>Tree based on Id</returns>
    public Tree SelectTree(int Id)
    {
        return dataBase.SelectTree(Id);
    }
    /// <summary>
    /// adds a comment to a tree
    /// </summary>
    /// <param name="tree">tree to have comment added</param>
    /// <param name="comment">comment to be added</param>
    /// <returns>string of error or worked</returns>
    /// <returns>string of error or worked</returns>
    public String AddCommentToTree(Tree tree, Comment comment)
    {
        return dataBase.AddCommentToTree(tree, comment);
    }

}
