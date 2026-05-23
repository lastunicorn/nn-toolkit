using System.Text.Json;
using DustInTheWind.NN.Toolkit.MandatoryPrivatePension;

namespace DustInTheWind.NN.Toolkit.Cli.Ports.DataAccess;

public class ContributionPersister : IEntityPersister<Contribution>
{
    private const string DatabasePath = "Data";
    private const string FileName = "contributions.json";

    private readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        WriteIndented = true,
        Converters = { new MonthDateJsonConverter() }
    };

    public IEnumerable<Contribution> Load()
    {
        string filePath = Path.Combine(DatabasePath, FileName);

        if (!File.Exists(filePath))
            return [];

        string json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<List<Contribution>>(json, jsonSerializerOptions) ?? [];
    }

    public void Save(IEnumerable<Contribution> contributions)
    {
        if (!Directory.Exists(DatabasePath))
            Directory.CreateDirectory(DatabasePath);

        string filePath = Path.Combine(DatabasePath, FileName);
        string json = JsonSerializer.Serialize(contributions, jsonSerializerOptions);
        File.WriteAllText(filePath, json);
    }
}