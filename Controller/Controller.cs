using Kompetenzcheck.Controller;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

public class VehicleController
{
    private readonly DBConnector _db;

    public VehicleController()
    {
        _db = new DBConnector();
    }

    // CRUD Operations
    public async Task<List<Fahrzeug>> GetAllVehiclesAsync()
    {
        return await _db.ExecuteQueryAsync("SELECT * FROM Fahrzeug");
    }

    // LINQ Filters
    public async Task<List<Fahrzeug>> GetVehiclesByManufacturerAsync(int herstellerId)
    {
        var vehicles = await GetAllVehiclesAsync();
        return vehicles.Where(v => v.HerstellerID == herstellerId).ToList();
    }

    public async Task<List<Fahrzeug>> GetVehiclesByPriceRangeAsync(float min, float max)
    {
        var vehicles = await GetAllVehiclesAsync();
        return vehicles.Where(v => v.Preis >= min && v.Preis <= max).ToList();
    }

    public async Task<List<Fahrzeug>> GetVehiclesByCategoryAsync(string category)
    {
        var vehicles = await GetAllVehiclesAsync();
        return vehicles.Where(v => v.Kategorie == category).ToList();
    }

    public async Task<List<Fahrzeug>> GetTopNewestVehiclesAsync(int count)
    {
        var vehicles = await GetAllVehiclesAsync();
        return vehicles.OrderByDescending(v => v.Baujahr).Take(count).ToList();
    }

    public async Task SaveVehicleAsync(Fahrzeug vehicle)
    {
        string query;
        var parameters = new Dictionary<string, object>
    {
        { "@bezeichnung", vehicle.Bezeichnung },
        { "@baujahr", vehicle.Baujahr },
        { "@preis", vehicle.Preis },
        { "@kategorie", vehicle.Kategorie },
        { "@herstellerId", vehicle.HerstellerID }
    };

        if (vehicle.FahrzeugID == 0)
        {
            query = "INSERT INTO Fahrzeuge (Bezeichnung, Baujahr, Preis, Kategorie, HerstellerID) " +
                    "VALUES (@bezeichnung, @baujahr, @preis, @kategorie, @herstellerId)";
        }
        else
        {
            query = "UPDATE Fahrzeuge SET Bezeichnung=@bezeichnung, Baujahr=@baujahr, " +
                    "Preis=@preis, Kategorie=@kategorie, HerstellerID=@herstellerId " +
                    "WHERE FahrzeugID=@id";
            parameters.Add("@id", vehicle.FahrzeugID);
        }

        await _db.ExecuteQueryAsync(query, parameters);
    }

}