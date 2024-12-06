using Kompetenzcheck.Controller;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System;

internal class DBConnector
{
    // attribut connection string
    private readonly string _connectionString;

    public DBConnector()
    {
        // Read from app.config
        _connectionString = ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString;
    }

    private MySqlConnection CreateConnection()
    {
        // return connection string
        return new MySqlConnection(_connectionString);
    }

    // Method that takes a query string, a Dictionary 
    public async Task<List<Fahrzeug>> ExecuteQueryAsync(string query, Dictionary<string, object> parameters = null)
    {
        var vehicles = new List<Fahrzeug>();
        using (var conn = CreateConnection())
        {
            await conn.OpenAsync();
            using (var cmd = new MySqlCommand(query, conn))
            {
                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        cmd.Parameters.AddWithValue(param.Key, param.Value);
                    }
                }
                using (var reader = (MySqlDataReader)await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        vehicles.Add(MapVehicle(reader));
                    }
                }
            }
        }
        return vehicles;
    }

    private Fahrzeug MapVehicle(MySqlDataReader reader)
    {
        return new Fahrzeug
        {
            FahrzeugID = reader.GetInt32("FahrzeugID"),
            Bezeichnung = reader.GetString("Bezeichnung"),
            Baujahr = reader.GetInt32("Baujahr"),
            Preis = reader.GetFloat("Preis"),
            Kategorie = reader.GetString("Kategorie"),
            HerstellerID = reader.GetInt32("HerstellerID"),
            
        };
    }
}