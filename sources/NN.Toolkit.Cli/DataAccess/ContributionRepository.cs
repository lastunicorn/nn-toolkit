using System.Text.Json;
using DustInTheWind.NN.Toolkit.MandatoryPrivatePension;

namespace DustInTheWind.NN.Toolkit.Cli.DataAccess;

public class ContributionRepository
{
    private const string DatabasePath = "Data";
    private const string FileName = "contributions.json";

    private readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        WriteIndented = true,
        Converters = { new MonthDateJsonConverter() }
    };

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

    public void Clear()
    {
        contributions.Clear();
    }

    public void SaveChanges()
    {
        SaveContributions();
    }

    private void LoadContributions()
    {
        string filePath = Path.Combine(DatabasePath, FileName);

        if (!File.Exists(filePath))
            return;

        string json = File.ReadAllText(filePath);
        contributions = JsonSerializer.Deserialize<List<Contribution>>(json, jsonSerializerOptions) ?? [];
    }

    private void SaveContributions()
    {
        if (!Directory.Exists(DatabasePath))
            Directory.CreateDirectory(DatabasePath);

        string filePath = Path.Combine(DatabasePath, FileName);
        string json = JsonSerializer.Serialize(contributions, jsonSerializerOptions);
        File.WriteAllText(filePath, json);
    }
}