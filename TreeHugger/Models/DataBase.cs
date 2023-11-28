using Npgsql;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows.Markup;
using TreeHugger.Interfaces;
using TreeHugger.Models;
using static TreeHugger.Models.ErrorReporting;

public class DataBase : IDataBase
{
    private String connString = GetConnectionString();
    ObservableCollection<Tree> trees = new();
    public ObservableCollection<Tree> Trees
    {
        get { return trees; }
    }
    public ObservableCollection<Item> Items
    {
        get { return SelectAllItems(); }
    }
    static String GetConnectionString()
    {
        var connStringBuilder = new NpgsqlConnectionStringBuilder();
        connStringBuilder.Host = "nimble-mummy-13052.5xj.cockroachlabs.cloud";
        connStringBuilder.Port = 26257;
        connStringBuilder.SslMode = SslMode.VerifyFull;
        connStringBuilder.Username = "hawleyia47"; // won't hardcode this in your app
        connStringBuilder.Password = "NuMH9TrulgojQd7kWuVNLg";
        connStringBuilder.Database = "defaultdb";
        connStringBuilder.ApplicationName = "whatever"; // ignored, apparently
        connStringBuilder.IncludeErrorDetail = true;
        return connStringBuilder.ConnectionString;
    }

    public void InitializeDatabase()
    {
        CreateTreeTable(connString);

    }
    /// <summary>
    /// Creates tree table in our database to add new trees
    /// </summary>
    /// <param name="connString"></param>
    static void CreateTreeTable(string connString)
    {
        using var conn = new NpgsqlConnection(connString); // a conn represents a connection to the database
        conn.Open(); // open the connection ... now we are connected!
        new NpgsqlCommand("CREATE TABLE IF NOT EXISTS trees (id INT,caption varchar,speciesId INT,location VARCHAR,latitude varchar,longitude varchar,image BYTEA);", conn).ExecuteNonQuery();
    }
    /// <summary>
    /// Creates species tree in our database to add new species of trees
    /// </summary>
    /// <param name="connString">string connString</param>
    static void CreateSpeciesTable(string connString)
    {
        using var conn = new NpgsqlConnection(connString); // a conn represents a connection to the database
        conn.Open(); // open the connection ... now we are connected!
        new NpgsqlCommand("CREATE TABLE species IF NOT EXISTS species(id INT, name varchar, locations varchar, color varchar, example_image BYTEA);", conn).ExecuteNonQuery();
    }
    /// <summary>
    /// Creates chat table in our database to add new chats to each tree
    /// </summary>
    /// <param name="connString">string connString</param>
    static void CreateChatTable(string connString)
    {
        using var conn = new NpgsqlConnection(connString); // a conn represents a connection to the database
        conn.Open(); // open the connection ... now we are connected!
        new NpgsqlCommand("CREATE TABLE chats(id INT, message varchar, date_submitted date);", conn).ExecuteNonQuery();
    }
    /// <summary>
    /// Creates avatar accessories table in our database 
    /// </summary>
    /// <param name="connString">string connString</param>
    static void CreateAvatarAccessoriesTable(string connString)
    {
        using var conn = new NpgsqlConnection(connString); // a conn represents a connection to the database
        conn.Open(); // open the connection ... now we are connected!
        new NpgsqlCommand("CREATE TABLE avatar_accessories(id INT, name varchar, cost INT,xOffset INT, yOffset INT, xSize INT, ySize INT, image BYTEA);", conn).ExecuteNonQuery();
    }
    /// <summary>
    /// adds accessory to database
    /// </summary>
    /// <param name="item">avatar accessory to be added</param>
    /// <returns>Boolean</returns>
    public Boolean AddAvatarAccessory(Item item)
    {
        //Item(string name, int cost, int xOffset, int yOffset, int xSize,int ySize, Byte[] image)
        try
        {
            using var conn = new NpgsqlConnection(connString); // conn, short for connection, is a connection to the database
            conn.Open(); // open the connection ... now we are connected!
            var cmd = new NpgsqlCommand("INSERT INTO avatar_accessories (id INT, name varchar, cost INT,xOffset INT, yOffset INT, xSize INT, ySize INT, image BYTEA) VALUES (@id, @name, @cost, @xOffset, @yOffset, @xSize, @ySize, @image)", conn);
            cmd.Parameters.AddWithValue("id", item.Id);
            cmd.Parameters.AddWithValue("cost", item.Cost);
            cmd.Parameters.AddWithValue("name", item.Name);
            cmd.Parameters.AddWithValue("xOffset", item.XOffset);
            cmd.Parameters.AddWithValue("yOffset", item.YOffset);
            cmd.Parameters.AddWithValue("xSize", item.XSize);
            cmd.Parameters.AddWithValue("ySize", item.YSize);
            cmd.Parameters.AddWithValue("image", item.Image);
            cmd.ExecuteNonQuery(); // used for INSERT, UPDATE & DELETE statements - returns # of affected rows
        }
        catch (Npgsql.PostgresException pe)
        {
            Console.WriteLine("Insert failed, {0}", pe);
            return false;
        }
        return true;
    }
    /// <summary>
    /// Get all tree species from the database.
    /// </summary>
    /// <returns>ObservableCollection<Species></returns>
    public ObservableCollection<Species> SelectAllSpecies()
    {
        ObservableCollection<Species> species = new ObservableCollection<Species>();
        var conn = new NpgsqlConnection(connString);
        conn.Open();
        // using() ==> disposable types are properly disposed of, even if there is an exception thrown
        using var cmd = new NpgsqlCommand("SELECT * FROM species;", conn);
        using var reader = cmd.ExecuteReader(); // used for SELECT statement, returns a forward-only traversable object
        while (reader.Read()) // each time through we get another row in the table (i.e., another Tree)
        {
            int id = reader.GetInt32(0);
            string name = reader.GetString(1);
            string locationsFound = reader.GetString(2);
            string color = reader.GetString(3);
            Byte[] exampleImage = (byte[])reader["image"];
            Species speciesToAdd = new Species(id, 
                name, 
                locationsFound, 
                color, 
                exampleImage);
            species.Add(speciesToAdd);
            
        }
        return species;
    }
    /// <summary>
    /// gets all items from database
    /// </summary>
    /// <returns>ObservableCollection<Item></returns>
    public ObservableCollection<Item> SelectAllItems()
    {
        ObservableCollection<Item> items = new ObservableCollection<Item>();
        var conn = new NpgsqlConnection(connString);
        conn.Open();
        Item itemToAdd = null;
        // using() ==> disposable types are properly disposed of, even if there is an exception thrown
        using var cmd = new NpgsqlCommand("SELECT id, name, cost, xOffset, yOffset, xSize, ySize, image FROM avatar_accessories;", conn);
        using var reader = cmd.ExecuteReader(); // used for SELECT statement, returns a forward-only traversable object
        while (reader.Read()) // each time through we get another row in the table (i.e., another Tree)
        {
            int Id = reader.GetInt32(0);
            string Name = reader.GetString(1);
            int Cost = reader.GetInt32(2);
            int xOffset = reader.GetInt32(3);
            int yOffset = reader.GetInt32(4);
            int xSize = reader.GetInt32(5);
            int ySize = reader.GetInt32(6);
            byte[] image = (byte[])reader["image"];
            itemToAdd = new(Id, Name, Cost, xOffset, yOffset, xSize, ySize, image);
            items.Add(itemToAdd);
        }
        return items;
    }
    /// <summary>
    /// INserts a tree into the database.
    /// </summary>
    /// <param name="tree"></param>
    /// <returns>Boolean</returns>
    public Boolean InsertTree(Tree tree)
    {
        try
        {
            using var conn = new NpgsqlConnection(connString); // conn, short for connection, is a connection to the database
            conn.Open(); // open the connection ... now we are connected!
            var cmd = new NpgsqlCommand("INSERT INTO trees (id, species_id, location, latitude, longitude, image) VALUES (@id, @species_id, @location, @latitude, @longitude, @image)", conn);
            cmd.Parameters.AddWithValue("id", tree.Id);
            cmd.Parameters.AddWithValue("species_id", tree.SpeciesId);
            cmd.Parameters.AddWithValue("location", tree.Location);
            cmd.Parameters.AddWithValue("latitude", tree.Latitude);
            cmd.Parameters.AddWithValue("longitude", tree.Longitude);
            cmd.Parameters.AddWithValue("image", tree.Image);
            cmd.ExecuteNonQuery(); // used for INSERT, UPDATE & DELETE statements - returns # of affected rows
            
        }
        catch (Npgsql.PostgresException pe)
        {
            Console.WriteLine("Insert failed, {0}", pe);
            return false;
        }
        return true;
    }
    /// <summary>
    /// Adds a species into the database
    /// </summary>
    /// <param name="species">Species</param>
    /// <returns>SpeciesAdditionError</returns>
    public SpeciesAdditionError InsertSpecies(Species species)
    {
        try
        {
            using var conn = new NpgsqlConnection(connString); // conn, short for connection, is a connection to the database
            conn.Open(); // open the connection ... now we are connected!
            var cmd = new NpgsqlCommand("INSERT INTO species(id, name, locations, color, example_image) VALUES (@id, @name, @color, @example_image)", conn);
            cmd.Parameters.AddWithValue("id", species.Id);
            cmd.Parameters.AddWithValue("name", species.Name);
            cmd.Parameters.AddWithValue("color", species.Color);
            cmd.Parameters.AddWithValue("example_image", species.ExampleImage);
            cmd.ExecuteNonQuery(); // used for INSERT, UPDATE & DELETE statements - returns # of affected rows
            SelectAllSpecies();
        }
        catch (Npgsql.PostgresException pe)
        {

            Console.WriteLine("Species insert failed {0}", pe);
            return SpeciesAdditionError.DBAdditionError;

        }
        return SpeciesAdditionError.NoError;
    }
    public EditTreeError UpdateTree(Tree tree) 
    {
        try
        {
            using var conn = new NpgsqlConnection(connString); // conn, short for connection, is a connection to the database

            conn.Open(); // open the connection ... now we are connected
            var cmd = new NpgsqlCommand("UPDATE trees SET species_id = @species_id, location = @location, latitude = @latitude, longitude = @longitude, image = @image WHERE id = @id", conn);

            cmd.Parameters.AddWithValue("id", tree.Id);
            cmd.Parameters.AddWithValue("species_id", tree.SpeciesId);
            cmd.Parameters.AddWithValue("location", tree.Location);
            cmd.Parameters.AddWithValue("latitude", tree.Latitude);  // Corrected the spelling of Latitude
            cmd.Parameters.AddWithValue("longitude", tree.Longitude);
            cmd.Parameters.AddWithValue("image", tree.Image);
            var numAffected = cmd.ExecuteNonQuery();

            SelectAllTrees();
        }
        catch (Npgsql.PostgresException pe)
        {
            Console.WriteLine("Update failed, {0}", pe);
            return EditTreeError.DBEditError;
        }
        return EditTreeError.NoError;
    }

    /// <summary>
    /// SelectAllTrees
    /// Gets all trees from the database.
    /// </summary>
    /// <returns>ObservableCollection<Tree></returns>
    public ObservableCollection<Tree> SelectAllTrees()
    {
        trees.Clear();
        var conn = new NpgsqlConnection(connString);
        conn.Open();
        Tree treeToAdd = null;
        // using() ==> disposable types are properly disposed of, even if there is an exception thrown
        using var cmd = new NpgsqlCommand("SELECT id, species_id, location, latitude, longitude, image FROM trees;", conn);
        using var reader = cmd.ExecuteReader(); // used for SELECT statement, returns a forward-only traversable object
        while (reader.Read()) // each time through we get another row in the table (i.e., another Tree)
        {
            int Id = reader.GetInt32(0);
            int speciesId = reader.GetInt32(1);
            String location = reader.GetString(2);
            double latitude = reader.GetDouble(3);
            double longitude = reader.GetDouble(4);
            byte[] image = (byte[])reader["image"];
            treeToAdd = new Tree(Id, speciesId, location, latitude, longitude, image);
            trees.Add(treeToAdd);
            Console.WriteLine(treeToAdd); // Log the retrieved tree
        }
        return trees;
    }
    /// <summary>
    /// Select Tree
    /// Returns null if tree is not found.
    /// </summary>
    /// <param name="Id"></param>
    /// <returns>Tree</returns>
    public Tree SelectTree(int Id)
    {
        var conn = new NpgsqlConnection(connString);
        conn.Open();
        // using() ==> disposable types are properly disposed of, even if there is an exception thrown
        using var cmd = new NpgsqlCommand($"SELECT id, species_id, location, latitude, longitude, image FROM trees WHERE id = '{Id}'", conn);
        using var reader = cmd.ExecuteReader(); // used for SELECT statement, returns a forward-only traversable object
        while (reader.Read()) // each time through we get another row in the table (i.e., another Airport)
        {
            Tree returnTree;
            int _Id = reader.GetInt32(0);
            int speciesId = reader.GetInt32(1);
            String location = reader.GetString(2);
            double  latitude = reader.GetDouble(3);
            double longitude = reader.GetDouble(4);
            byte[] image = (byte[])reader["image"];
            returnTree = new Tree(_Id, speciesId, location, latitude, longitude, image);
            if (returnTree != null)
            {
                Console.WriteLine($"Select Tree returned tree: " + Id); // Log the retrieved tree
                return returnTree;
            }
            Console.WriteLine($"Select tree did not find an airport with an id of:{Id}");
            return null; // Tree not found.

        }
        return null; // Reader failed.

    }
    /// <summary>
    /// Get the max tree id from the database and add one.
    /// This gets around auto increment in the database.
    /// </summary>
    /// <returns>int</returns>
    public int GetMaxTreeId()
    {
        var conn = new NpgsqlConnection(connString);
        conn.Open();
        // using() ==> disposable types are properly disposed of, even if there is an exception thrown
        using var cmd = new NpgsqlCommand($"SELECT MAX(id) FROM trees", conn);
        using var reader = cmd.ExecuteReader(); // used for SELECT statement, returns a forward-only traversable object
        int returnId = 0;
        while (reader.Read()) // each time through we get another row in the table (i.e., another Airport)
        {
            returnId = reader.GetInt32(0);
            if(returnId <= 0)
            {
                return 1;
            }
            

        }
        returnId++; // Add 1 to the id. Cockroach db does not have an autoincrement featrue 
        return returnId; // Reader failed.
    }

    public EditSpeciesError UpdateSpecies(Species species)
    {
        try
        {
            using var conn = new NpgsqlConnection(connString); // conn, short for connection, is a connection to the database

            conn.Open(); // open the connection ... now we are connected
            var cmd = new NpgsqlCommand("UPDATE species SET name = @name, color = @color, example_image = @example_image WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("id", species.Id);
            cmd.Parameters.AddWithValue("name", species.Name);
            cmd.Parameters.AddWithValue("color", species.Color);
            cmd.Parameters.AddWithValue("example_image", species.ExampleImage);
            cmd.ExecuteNonQuery();
        }
        catch (Npgsql.PostgresException pe)
        {
            Console.WriteLine("Update failed, {0}", pe);
            return EditSpeciesError.DBEditError;
        }
        return EditSpeciesError.NoError;
    }
    /// <summary>
    /// Delete Tree
    /// Returns true if the delete was completed successfully.
    /// </summary>
    /// <param name="treeToDelete">Tree</param>
    /// <returns>Boolean</returns>
    public Boolean DeleteTre(Tree treeToDelete)
    {
        var conn = new NpgsqlConnection(connString);
        conn.Open();
        using var cmd = new NpgsqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = "DELETE FROM trees WHERE id = @id";
        cmd.Parameters.AddWithValue("id", treeToDelete.Id);
        int numDeleted = cmd.ExecuteNonQuery();
        if (numDeleted > 0)
        {
            SelectAllTrees(); //If we delete an tree, we want our ObservableCollection to "refetch", so it's no longer displayed
        }
        return numDeleted > 0;
    }
}