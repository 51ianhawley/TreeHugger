﻿using Microsoft.Maui.Controls.Maps;
using Npgsql;
using System.Collections.ObjectModel;
using System.Data;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Markup;
using TreeHugger.Interfaces;
using TreeHugger.Models;
using static TreeHugger.Models.ErrorReporting;

public class DataBase : IDataBase
{
    private String connString = GetConnectionString();

    ObservableCollection<Tree> trees = new();

    ObservableCollection<Pin> treePins = new();
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
    /// Creates tree table in our database to add new trees. TABLES HAS BEEN MODIFIED SINCE THIS METHOD WAS USED!!!
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
    /// Get all tree species from the database.
    /// </summary>
    /// <returns>ObservableCollection<Species></returns>
    public ObservableCollection<Species> SelectAllSpecies()
    {
        ObservableCollection<Species> species = new ObservableCollection<Species>();
        var conn = new NpgsqlConnection(connString);
        conn.Open();
        // using() ==> disposable types are properly disposed of, even if there is an exception thrown
        using var cmd = new NpgsqlCommand("SELECT * FROM species", conn);
        using var reader = cmd.ExecuteReader(); // used for SELECT statement, returns a forward-only traversable object
        while (reader.Read()) // each time through we get another row in the table (i.e., another Tree)
        {
            int id = reader.GetInt32(0);
            string name = reader.GetString(1);
            string locationsFound = reader.GetString(2);
            string color = reader.GetString(3);
            //String hexImage = reader.GetString(4);
            //byte[] exampleImage = ConvertHexStringToByteArray(hexImage);
            byte[] exampleImage = (byte[])reader["example_image"];

            Species speciesToAdd = new (id, 
                name, 
                locationsFound, 
                color, 
                exampleImage);
            species.Add(speciesToAdd);
            
        }
        return species;
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
            //cmd.Parameters.AddWithValue("comments", tree.GetComments());
            int rowsEffected = cmd.ExecuteNonQuery(); // used for INSERT, UPDATE & DELETE statements - returns # of affected rows
            
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
    public Boolean UpdateTree(Tree treeToUpdate, int speciesID, string location, string latitude, string longitude, Byte[] image)
    {
        try
        {
            using var conn = new NpgsqlConnection(connString); // conn, short for connection, is a connection to the database
            conn.Open(); // open the connection ... now we are connected!
            var cmd = new NpgsqlCommand(); // create the sql commaned
            cmd.Connection = conn; // commands need a connection, an actual command to execute
            cmd.CommandText = "UPDATE trees SET species_Id = @speciesId, location = @location, latitude = @latitude, longitude = @longitude, image = @image, comments = @comments WHERE id = @id;";
            cmd.Parameters.AddWithValue("id", treeToUpdate.Id);
            cmd.Parameters.AddWithValue("speciesId", speciesID);
            cmd.Parameters.AddWithValue("location", location);
            cmd.Parameters.AddWithValue("latitude", latitude);
            cmd.Parameters.AddWithValue("longitude", longitude);
            cmd.Parameters.AddWithValue("image", image);
            cmd.Parameters.AddWithValue("comments", treeToUpdate.GetComments());


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
        using var cmd = new NpgsqlCommand("SELECT id, species_id, location, latitude, longitude, comments, image FROM trees;", conn);
        using var reader = cmd.ExecuteReader(); // used for SELECT statement, returns a forward-only traversable object
        while (reader.Read()) // each time through we get another row in the table (i.e., another Tree)
        {
            int Id = reader.GetInt32(0);
            int speciesId = reader.GetInt32(1);
            String location = reader.GetString(2);
            string latitude = reader.GetString(3);
            string longitude = reader.GetString(4);
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
        using var cmd = new NpgsqlCommand($"SELECT id, species_id, location, latitude, longitude, comments, image  FROM trees WHERE id = '{Id}'", conn);
        using var reader = cmd.ExecuteReader(); // used for SELECT statement, returns a forward-only traversable object
        while (reader.Read()) // each time through we get another row in the table (i.e., another Airport)
        {
            Tree returnTree;
            int id = reader.GetInt32(0);
            int speciesId = reader.GetInt32(1);
            String location = reader.GetString(2);
            string  latitude = reader.GetString(3);
            string longitude = reader.GetString(4);
            byte[] image = (byte[])reader["image"];
            if (reader.IsDBNull(5))
            {
                CommentsAreNull(Id);
                return SelectTree(Id);
            }
            string comments = reader.GetString(5);
            returnTree = new Tree(id, speciesId, location, latitude, longitude, image);
            if (returnTree != null)
            {
                Console.WriteLine($"Select Tree returned tree: " + Id); // Log the retrieved tree
                return returnTree;
            }
            Console.WriteLine($"Select tree did not find a tree with an id of:{Id}");
            return null; // Tree not found.

        }
        return null; // Reader failed.

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="Id"></param>
    /// <returns></returns>
    public String AddCommentToTree(Tree tree, Comment comment)
    {
        string jsonComments = tree.GetComments();
        ObservableCollection<Comment> comments = JsonSerializer.Deserialize<ObservableCollection<Comment>>(jsonComments);
        comments.Add(comment);
        jsonComments = JsonSerializer.Serialize<ObservableCollection<Comment>>(comments);
        try
        {
            using var conn = new NpgsqlConnection(connString); // conn, short for connection, is a connection to the database

            conn.Open(); // open the connection ... now we are connected
            var cmd = new NpgsqlCommand("UPDATE trees SET comments = @jsonComments WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("id", tree.Id);
            cmd.Parameters.AddWithValue("jsonComments", jsonComments);
            cmd.ExecuteNonQuery();
            SelectAllTrees();
        }
        catch (Npgsql.PostgresException pe)
        {
            return String.Format("Update failed, {0}", pe);
            //return EditSpeciesError.DBEditError;
        }

        return "worked";
        //return EditSpeciesError.NoError;

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
    public Boolean DeleteTree(Tree treeToDelete)
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
    /// <summary>
    /// Gets comments for a tree given an id
    /// </summary>
    /// <param name="Id">id for tree's comments wanted</param>
    /// <returns>observableCollection of Comments</returns>
    public ObservableCollection<Comment> GetComments(int Id)
    {
        Tree tree = SelectTree(Id);
        var conn = new NpgsqlConnection(connString);
        conn.Open();
        string jsonComments = null;
        // using() ==> disposable types are properly disposed of, even if there is an exception thrown
        using var cmd = new NpgsqlCommand($"SELECT comments FROM trees WHERE id = '{Id}';", conn);
        using var reader = cmd.ExecuteReader(); // used for SELECT statement, returns a forward-only traversable object
        while (reader.Read()) // each time through we get another row in the table (i.e., another Tree)
        {
            if (reader.IsDBNull(0))
            {
                return CommentsAreNull(Id);
            }
            else
            {
                jsonComments = reader.GetString(0);
            }
        }
        ObservableCollection<Comment> comments = new ObservableCollection<Comment>();
        comments = JsonSerializer.Deserialize<ObservableCollection<Comment>>(jsonComments);
        return comments;
    }
    public ObservableCollection<Comment> CommentsAreNull(int Id)
    {
        string jsonComments = JsonSerializer.Serialize<ObservableCollection<Comment>>(new ObservableCollection<Comment>());
        try
        {
            using var conn = new NpgsqlConnection(connString); // conn, short for connection, is a connection to the database

            conn.Open(); // open the connection ... now we are connected
            var cmd = new NpgsqlCommand("UPDATE trees SET comments = @jsonComments WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("id", Id);
            cmd.Parameters.AddWithValue("jsonComments", jsonComments);
            int rowsEffected = cmd.ExecuteNonQuery();
        }
        catch (Npgsql.PostgresException pe)
        {
            //return String.Format("Update failed, {0}", pe);
            //return EditSpeciesError.DBEditError;
        }

        return GetComments(Id);
        //return EditSpeciesError.NoError;

    }

    /// <summary>
    /// Generates all tree pins for the map page
    /// </summary>
    /// <returns></returns>
    public ObservableCollection<Pin> GenerateAllTeePins()
    {
        var conn = new NpgsqlConnection(connString);
        conn.Open();

        // using() ==> disposable types are properly disposed of, even if there is an exception thrown 
        using var cmd = new NpgsqlCommand("SELECT t.id, t.species_id, t.latitude, t.longitude, s.name FROM trees t JOIN public.species s on s.id = t.species_id;", conn);
        using var reader = cmd.ExecuteReader(); // used for SELECT statement, returns a forward-only traversable object

        while (reader.Read()) // each time through we get another row in the table (i.e., another Airport)
        {
            int id = reader.GetInt32(0);
            int speciesId = reader.GetInt32(1);

            String latitude = reader.GetString(2);
            String longitude = reader.GetString(3);
            String speciesName = reader.GetString(4);
            Location location = new(Double.Parse(latitude), Double.Parse(longitude));
            Pin treePinToAdd = new() { Label = speciesName, Location = location };
            treePins.Add(treePinToAdd);
            Console.WriteLine(treePinToAdd);
        }

        return treePins;
    }
    /// <summary>
    /// Converts images out of a hex string.
    /// </summary>
    /// <param name="hexString"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    private static byte[] ConvertHexStringToByteArray(string hexString)
    {
        if (hexString.Length % 2 != 0)
        {
            throw new ArgumentException("Hex string must have an even length", "hexString");
        }

        var bytes = new byte[hexString.Length / 2];
        for (int i = 0; i < hexString.Length; i += 2)
        {
            bytes[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
        }
        return bytes;
    }

}