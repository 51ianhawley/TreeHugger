using Npgsql;
using System.Collections.ObjectModel;
using System.Data;
using TreeHugger.Interfaces;
using TreeHugger.Models;

public class DataBase : IDataBase
{
    private String connString = GetConnectionString();
    ObservableCollection<Tree> trees = new();
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
            cmd.Parameters.AddWithValue("latitude", tree.Lattiude);
            cmd.Parameters.AddWithValue("longitude", tree.Longitude);
            cmd.Parameters.AddWithValue("image", tree.Image);
            cmd.ExecuteNonQuery(); // used for INSERT, UPDATE & DELETE statements - returns # of affected rows
            SelectAllTrees();
        }
        catch (Npgsql.PostgresException pe)
        {
            Console.WriteLine("Insert failed, {0}", pe);
            return false;
        }
        return true;
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
            int Id = int.Parse(reader.GetString(0));
            int speciesId = int.Parse(reader.GetString(1));
            String location = reader.GetString(2);
            string latitude = reader.GetString(3);
            string longitude = reader.GetString(4);
            byte[] image = (byte[])reader["image"];
            treeToAdd = new(Id, speciesId, location, latitude, longitude, image);
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
            int _Id = int.Parse(reader.GetString(0));
            int speciesId = int.Parse(reader.GetString(1));
            String location = reader.GetString(2);
            string latitude = reader.GetString(3);
            string longitude = reader.GetString(4);
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


    public Boolean UpdateTree(Tree treeToUpdate, int speciesID, string location, string latitude, string longitude, Byte[] image)
    {
        try
        {
            using var conn = new NpgsqlConnection(connString); // conn, short for connection, is a connection to the database
            conn.Open(); // open the connection ... now we are connected!
            var cmd = new NpgsqlCommand(); // create the sql commaned
            cmd.Connection = conn; // commands need a connection, an actual command to execute
            cmd.CommandText = "UPDATE airports SET city = @city, date_visited = @dateVisited, rating = @rating WHERE id = @id;";
            cmd.Parameters.AddWithValue("id", treeToUpdate.Id);
            cmd.Parameters.AddWithValue("speciesId", speciesID);
            cmd.Parameters.AddWithValue("location", location);
            cmd.Parameters.AddWithValue("latitude", latitude);
            cmd.Parameters.AddWithValue("longitude", longitude);
            cmd.Parameters.AddWithValue("image", image);


            var numAffected = cmd.ExecuteNonQuery();
            SelectAllTrees();
        }
        catch (Npgsql.PostgresException pe)
        {
            Console.WriteLine("Update failed, {0}", pe);
            return false;
        }
        return true;
    }
    /// <summary>
    /// Delete Tree
    /// Returns true if the delete was completed successfully.
    /// </summary>
    /// <param name="treeToDelete"></param>
    /// <returns>Boolean</returns>
    public Boolean DeleteAirport(Tree treeToDelete)
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