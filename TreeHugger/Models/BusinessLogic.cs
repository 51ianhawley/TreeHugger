using System.Collections.ObjectModel;

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
}
