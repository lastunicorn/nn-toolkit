using Tabula;
using Tabula.Detectors;
using Tabula.Extractors;
using UglyToad.PdfPig;

namespace DustInTheWind.NN.Toolkit.P2.Pdf;

internal class P2PdfDocument
{
    private readonly string filePath;

    public P2PdfDocument(string filePath)
    {
        this.filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
    }

    public IEnumerable<P2PdfTableRow> EnumerateRows()
    {
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

            IReadOnlyList<Table> tables = ExtractTables(page, regions, algorithm);

            foreach (Table table in tables)
            {
                List<P2PdfTableRow> rows = table.Rows
                    .Select(x => new P2PdfTableRow(x))
                    .ToList();

                if (rows.Count == 0)
                    continue;

                foreach (P2PdfTableRow row in rows)
                   yield return row;
            }
        }
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
}