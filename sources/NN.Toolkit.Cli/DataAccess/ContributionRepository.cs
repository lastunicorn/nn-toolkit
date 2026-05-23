using System.Text.Json;
using DustInTheWind.NN.Toolkit.MandatoryPrivatePension;

namespace DustInTheWind.NN.Toolkit.Cli.DataAccess;

public class ContributionRepository
{
    private readonly string databasePath = "Data";
    private readonly string fileName = "contributions.json";
    private List<Contribution> contributions = [];

    public ContributionRepository()
    {
        LoadContributions();
    }

    public IEnumerable<Contribution> GetAll()
    {
        return contributions;
    }

    public Contribution Get(MonthDate contributionMonth)
    {
        return contributions.FirstOrDefault(x => x.Month == contributionMonth);
    }

    public void Add(Contribution contribution)
    {
        if (contribution == null) throw new ArgumentNullException(nameof(contribution));

        contributions.Add(contribution);
    }

    public void SaveChanges()
    {
        SaveContributions();
    }

    private void LoadContributions()
    {
        string filePath = Path.Combine(databasePath, fileName);

        if (!File.Exists(filePath))
            return;

        string json = File.ReadAllText(filePath);
        contributions = JsonSerializer.Deserialize<List<Contribution>>(json) ?? [];
    }

    private void SaveContributions()
    {
        if (!Directory.Exists(databasePath))
            Directory.CreateDirectory(databasePath);

        string filePath = Path.Combine(databasePath, fileName);
        JsonSerializerOptions jsonSerializerOptions = new() { WriteIndented = true };
        string json = JsonSerializer.Serialize(contributions, jsonSerializerOptions);
        File.WriteAllText(filePath, json);
    }
}