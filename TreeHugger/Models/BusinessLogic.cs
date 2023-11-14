using System.Collections.ObjectModel;

namespace TreeHugger.Models;

public class BusinessLogic
{
    DataBase dataBase;
    public ObservableCollection<Tree> Trees 
    {
        get { return dataBase.SelectAllTrees(); }
    }

    public DataBase DataBase { get { return dataBase; } }

    public BusinessLogic()
    {
        dataBase = new DataBase();
    }
}
