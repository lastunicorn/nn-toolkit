using System.Collections.ObjectModel;
using System.Globalization;
using Tabula;
using Tabula.Detectors;
using Tabula.Extractors;
using UglyToad.PdfPig;

namespace DustInTheWind.NN.Toolkit.Pilonul2;

public class ContributionsDocument : Collection<Contribution>
{
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

        for (int pageIndex = 1; pageIndex <= pdfDocument.NumberOfPages; pageIndex++)
        {
            PageArea page = ObjectExtractor.Extract(pdfDocument, pageIndex);
            IReadOnlyList<TableRectangle> regions = detector.Detect(page);

            foreach (TableRectangle region in regions)
            {
                IReadOnlyList<Table> tables = algorithm.Extract(page.GetArea(region.BoundingBox));

                foreach (Table table in tables)
                {
                    IEnumerable<PdfTableRow> rows = table.Rows.Select(x => new PdfTableRow(x));

                    if (pageIndex == 1)
                    {
                        document.ColumnNames.AddRange(rows.First());
                        rows = rows.Skip(1);
                    }

                    foreach (PdfTableRow row in rows)
                    {
                        Contribution contribution = new()
                        {
                            Month = row[0],
                            GrossValue = ParseDecimal(row[1]),
                            AdministrationFee = ParseDecimal(row[2]),
                            NetValue = ParseDecimal(row[3]),
                            UnitValue = ParseDecimal(row[4]),
                            UnitCount = ParseDecimal(row[5]),
                            PaidInMonth = row[6]
                        };

                        document.Add(contribution);
                    }
                }
            }
        }

        return document;
    }

    private static decimal ParseDecimal(string value)
    {
        return decimal.Parse(value.Trim(), CultureInfo.InvariantCulture);
    }
}