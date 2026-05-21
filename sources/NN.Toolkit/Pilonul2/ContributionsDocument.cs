using System.Collections.ObjectModel;
using System.Globalization;
using System.Text.RegularExpressions;
using Tabula;
using Tabula.Detectors;
using Tabula.Extractors;
using UglyToad.PdfPig;

namespace DustInTheWind.NN.Toolkit.Pilonul2;

public class ContributionsDocument : Collection<Contribution>
{
    private static readonly Regex Pattern = new(@"\s+", RegexOptions.IgnoreCase | RegexOptions.Compiled);

    public List<string> ColumnNames { get; } = [];

    public static ContributionsDocument LoadFromFile(string filePath)
    {
        ContributionsDocument document = [];

        ParsingOptions options = new()
        {
            ClipPaths = true
        };

        using PdfDocument pdfDocument = PdfDocument.Open(filePath, options);

        SimpleNurminenDetectionAlgorithm detector = new();
        IExtractionAlgorithm algorithm = new BasicExtractionAlgorithm();
        bool hasCapturedHeader = false;

        for (int pageIndex = 1; pageIndex <= pdfDocument.NumberOfPages; pageIndex++)
        {
            PageArea page = ObjectExtractor.Extract(pdfDocument, pageIndex);
            IReadOnlyList<TableRectangle> regions = detector.Detect(page);

            IReadOnlyList<Table> tables = ExtractTables(page, regions, algorithm);

            foreach (Table table in tables)
            {
                List<PdfTableRow> rows = table.Rows
                    .Select(x => new PdfTableRow(x))
                    .ToList();

                if (rows.Count == 0)
                    continue;

                int startIndex = 0;

                if (!hasCapturedHeader)
                {
                    IEnumerable<string> columnNames = rows[0]
                        .Select(x => Pattern.Replace(x, " "));

                    document.ColumnNames.AddRange(columnNames);
                    hasCapturedHeader = true;
                    startIndex = 1;
                }

                foreach (PdfTableRow row in rows.Skip(startIndex))
                {
                    if (TryCreateContribution(row, out Contribution contribution))
                        document.Add(contribution);
                }
            }
        }

        return document;
    }

    private static IReadOnlyList<Table> ExtractTables(PageArea page, IReadOnlyList<TableRectangle> regions, IExtractionAlgorithm algorithm)
    {
        if (regions.Count == 0)
            return algorithm.Extract(page);

        List<Table> tables = [];

        foreach (TableRectangle region in regions)
            tables.AddRange(algorithm.Extract(page.GetArea(region.BoundingBox)));

        return tables;
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