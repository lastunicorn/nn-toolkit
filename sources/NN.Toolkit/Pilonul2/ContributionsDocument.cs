using System.Collections.ObjectModel;
using System.Globalization;
using System.Text.RegularExpressions;

namespace DustInTheWind.NN.Toolkit.Pilonul2;

public class ContributionsDocument : Collection<Contribution>
{
    private static readonly Regex Pattern = new(@"\s+", RegexOptions.IgnoreCase | RegexOptions.Compiled);

    public List<string> ColumnNames { get; } = [];

    public static ContributionsDocument LoadFromFile(string filePath)
    {
        ContributionsDocument document = [];

        P2PdfDocument pdfDocument = new(filePath);
        IEnumerable<PdfTableRow> rows = pdfDocument.EnumerateRows();

        bool hasCapturedHeader = false;

        foreach (PdfTableRow row in rows)
        {
            if (!hasCapturedHeader)
            {
                IEnumerable<string> columnNames = row
                    .Select(x => Pattern.Replace(x, " "));

                document.ColumnNames.AddRange(columnNames);
                hasCapturedHeader = true;
            }
            else
            {
                if (TryCreateContribution(row, out Contribution contribution))
                    document.Add(contribution);
            }
        }

        return document;
    }

    private static bool TryCreateContribution(PdfTableRow row, out Contribution contribution)
    {
        contribution = null!;

        if (!TryParseMonthDate(row[0], out MonthDate month) ||
            !TryParseDecimal(row[1], out decimal grossValue) ||
            !TryParseDecimal(row[2], out decimal administrationFee) ||
            !TryParseDecimal(row[3], out decimal netValue) ||
            !TryParseDecimal(row[4], out decimal unitValue) ||
            !TryParseDecimal(row[5], out decimal unitCount) ||
            !TryParseMonthDate(row[6], out MonthDate paidInMonth))
        {
            return false;
        }

        contribution = new Contribution
        {
            Month = month,
            GrossValue = grossValue,
            AdministrationFee = administrationFee,
            NetValue = netValue,
            UnitValue = unitValue,
            UnitCount = unitCount,
            PaidInMonth = paidInMonth
        };

        return true;
    }

    private static bool TryParseMonthDate(string value, out MonthDate monthDate)
    {
        try
        {
            monthDate = MonthDate.Parse(value.Trim());
            return true;
        }
        catch
        {
            monthDate = default;
            return false;
        }
    }

    private static bool TryParseDecimal(string value, out decimal decimalValue)
    {
        return decimal.TryParse(value.Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out decimalValue);
    }
}