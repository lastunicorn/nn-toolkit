using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace DustInTheWind.NN.Toolkit.MandatoryPrivatePension;

public class ContributionsHeader : Collection<string>
{
    private static readonly Regex Pattern = new(@"\s+", RegexOptions.IgnoreCase | RegexOptions.Compiled);

    public void AddRange(IEnumerable<string> values)
    {
        if (values == null) throw new ArgumentNullException(nameof(values));

        IEnumerable<string> columnNames = values
            .Select(x => x == null
                ? null
                : Pattern.Replace(x, " "));

        foreach (string columnName in columnNames)
            Items.Add(columnName);
    }
}