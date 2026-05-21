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

            IEnumerable<P2PdfTable> tables = ExtractTables(page, regions, algorithm);

            foreach (P2PdfTable table in tables)
            {
                IEnumerable<P2PdfTableRow> rows = table.EnumerateRows();

                foreach (P2PdfTableRow row in rows)
                    yield return row;
            }
        }
    }

    private static IEnumerable<P2PdfTable> ExtractTables(PageArea page, IReadOnlyList<TableRectangle> regions, IExtractionAlgorithm algorithm)
    {
        if (regions.Count == 0)
            yield break;

        foreach (TableRectangle region in regions)
        {
            PageArea pageArea = page.GetArea(region.BoundingBox);
            IEnumerable<P2PdfTable> p2PdfTables = algorithm.Extract(pageArea)
                .Select(x => new P2PdfTable(x));

            foreach (P2PdfTable p2PdfTable in p2PdfTables)
                yield return p2PdfTable;
        }
    }
}