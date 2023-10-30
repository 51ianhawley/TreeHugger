using Npgsql;
using System.Collections.ObjectModel;
using TreeHugger.Interfaces;

public class DataBase : IDataBase
{
    private String connString = GetConnectionString();

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

    //public void InitializeDatabase()
    //{
    //    CreateTable(connString);
    //}
    ///// <summary>
    ///// Creates table in our database to add new airports
    ///// </summary>
    ///// <param name="connString"></param>
    //static void CreateTable(string connString)
    //{
    //    using var conn = new NpgsqlConnection(connString); // a conn represents a connection to the database
    //    conn.Open(); // open the connection ... now we are connected!
    //    new NpgsqlCommand("CREATE TABLE IF NOT EXISTS airports(id VARCHAR(4) PRIMARY KEY, city VARCHAR(25), date_visited DATE, rating INT)", conn).ExecuteNonQuery();
    //}
    ///// <summary>
    ///// Insets into databse with given parameters for a new airport
    ///// </summary>
    ///// <param name="airport"></param>
    ///// <returns></returns>
    //public Boolean InsertAirport(Airport airport)
    //{
    //    try
    //    {
    //        using var conn = new NpgsqlConnection(connString); // conn, short for connection, is a connection to the database
    //        conn.Open(); // open the connection ... now we are connected!
    //        var cmd = new NpgsqlCommand("INSERT INTO airports (id, city, date_visited, rating) VALUES (@id, @city, @date_visited, @rating)", conn);
    //        cmd.Parameters.AddWithValue("id", airport.Id);
    //        cmd.Parameters.AddWithValue("city", airport.City);
    //        cmd.Parameters.AddWithValue("date_visited", airport.DateVisited);
    //        cmd.Parameters.AddWithValue("rating", airport.Rating);
    //        cmd.ExecuteNonQuery(); // used for INSERT, UPDATE & DELETE statements - returns # of affected rows
    //        SelectAllAirports();
    //    }
    //    catch (Npgsql.PostgresException pe)
    //    {
    //        Console.WriteLine("Insert failed, {0}", pe);
    //        return false;
    //    }
    //    return true;
    //}

    ///// <summary>
    ///// SelectAllAirports
    ///// Gets all airports from the database.
    ///// </summary>
    ///// <returns>ObservableCollection<Airport></returns>
    //public ObservableCollection<Airport> SelectAllAirports()
    //{
    //    airports.Clear();
    //    var conn = new NpgsqlConnection(connString);
    //    conn.Open();
    //    // using() ==> disposable types are properly disposed of, even if there is an exception thrown
    //    using var cmd = new NpgsqlCommand("SELECT id, city, date_visited, rating FROM airports", conn);
    //    using var reader = cmd.ExecuteReader(); // used for SELECT statement, returns a forward-only traversable object
    //    while (reader.Read()) // each time through we get another row in the table (i.e., another Airport)
    //    {
    //        String Id = reader.GetString(0);
    //        String City = reader.GetString(1);
    //        DateTime DateVisited = reader.GetDateTime(2);
    //        Int32 Rating = reader.GetInt32(3);
    //        Airport airportToAdd = new(Id, City, DateVisited, Rating);
    //        airports.Add(airportToAdd);
    //        Console.WriteLine(airportToAdd); // Log the retrieved airport
    //    }
    //    return airports;
    //}
    ///// <summary>
    ///// Select Airport
    ///// Returns null if airport is not found.
    ///// </summary>
    ///// <param name="Id"></param>
    ///// <returns>Airport</returns>
    //public Airport SelectAirport(String Id)
    //{
    //    var conn = new NpgsqlConnection(connString);
    //    conn.Open();
    //    // using() ==> disposable types are properly disposed of, even if there is an exception thrown
    //    using var cmd = new NpgsqlCommand($"SELECT id, city, date_visited, rating FROM airports WHERE id = '{Id}'", conn);
    //    using var reader = cmd.ExecuteReader(); // used for SELECT statement, returns a forward-only traversable object
    //    while (reader.Read()) // each time through we get another row in the table (i.e., another Airport)
    //    {
    //        Airport returnAirport;
    //        String ReturnId = reader.GetString(0);
    //        String City = reader.GetString(1);
    //        DateTime DateVisited = reader.GetDateTime(2);
    //        Int32 Rating = reader.GetInt32(3);
    //        returnAirport = new Airport(Id, City, DateVisited, Rating);
    //        if (returnAirport != null)
    //        {
    //            Console.WriteLine($"Select Airport returned airport: " + Id); // Log the retrieved airport
    //            return returnAirport;
    //        }
    //        Console.WriteLine($"Select airport did not find an airport with an id of:{Id}");
    //        return null; // Airport not found.

    //    }
    //    return null; // Reader failed.

    //}

    ///// <summary>
    ///// Delete Airport
    ///// Returns true if the delete was completed successfully.
    ///// </summary>
    ///// <param name="airportToDelete"></param>
    ///// <returns>Boolean</returns>
    //public Boolean UpdateAirport(Airport airportToUpdate, String city, DateTime dateVisited, int rating)
    //{
    //    try
    //    {
    //        using var conn = new NpgsqlConnection(connString); // conn, short for connection, is a connection to the database
    //        conn.Open(); // open the connection ... now we are connected!
    //        var cmd = new NpgsqlCommand(); // create the sql commaned
    //        cmd.Connection = conn; // commands need a connection, an actual command to execute
    //        cmd.CommandText = "UPDATE airports SET city = @city, date_visited = @dateVisited, rating = @rating WHERE id = @id;";
    //        cmd.Parameters.AddWithValue("id", airportToUpdate.Id);
    //        cmd.Parameters.AddWithValue("city", city);
    //        cmd.Parameters.AddWithValue("dateVisited", dateVisited); //double check caps
    //        cmd.Parameters.AddWithValue("rating", rating);
    //        var numAffected = cmd.ExecuteNonQuery();
    //        SelectAllAirports();
    //    }
    //    catch (Npgsql.PostgresException pe)
    //    {
    //        Console.WriteLine("Update failed, {0}", pe);
    //        return false;
    //    }
    //    return true;
    //}
    ///// <summary>
    ///// Delete Airport
    ///// Returns true if the delete was completed successfully.
    ///// </summary>
    ///// <param name="airportToDelete"></param>
    ///// <returns>Boolean</returns>
    //public Boolean DeleteAirport(Airport airportToDelete)
    //{
    //    var conn = new NpgsqlConnection(connString);
    //    conn.Open();
    //    using var cmd = new NpgsqlCommand();
    //    cmd.Connection = conn;
    //    cmd.CommandText = "DELETE FROM airports WHERE id = @id";
    //    cmd.Parameters.AddWithValue("id", airportToDelete.Id);
    //    int numDeleted = cmd.ExecuteNonQuery();
    //    if (numDeleted > 0)
    //    {
    //        SelectAllAirports(); //If we delete an airport, we want our ObservableCollection to "refetch", so it's no longer displayed
    //    }
    //    return numDeleted > 0;
    //}
}