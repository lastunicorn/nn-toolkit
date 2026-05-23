using System.Text.Json;
using DustInTheWind.NN.Toolkit.Cli.Domain;

namespace DustInTheWind.NN.Toolkit.Cli.DataAccess;

public class FundRecordPersister : IEntityPersister<FundRecord>
{
    private const string DatabasePath = "Data";
    private const string FileName = "fund-records.json";

    private readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        WriteIndented = true
    };

    public IEnumerable<FundRecord> Load()
    {
        string filePath = Path.Combine(DatabasePath, FileName);

        if (!File.Exists(filePath))
            return [];

        string json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<List<FundRecord>>(json, jsonSerializerOptions) ?? [];
    }

    public void Save(IEnumerable<FundRecord> fundRecords)
    {
        if (!Directory.Exists(DatabasePath))
            Directory.CreateDirectory(DatabasePath);

        string filePath = Path.Combine(DatabasePath, FileName);
        string json = JsonSerializer.Serialize(fundRecords, jsonSerializerOptions);
        File.WriteAllText(filePath, json);
    }
}

