using Tabula;
using Tabula.Detectors;
using Tabula.Extractors;

namespace DustInTheWind.NN.Toolkit.MandatoryPrivatePension.Pdf;

internal class P2PdfPage
{
    private readonly PageArea pageArea;

    public int PageIndex { get; }

    public TableExtractionApproach ExtractionApproach { get; private set; }

    public P2PdfPage(int pageIndex, PageArea pageArea)
    {
        PageIndex = pageIndex;
        this.pageArea = pageArea ?? throw new ArgumentNullException(nameof(pageArea));
    }

    public IEnumerable<P2PdfTable> EnumerateTables()
    {
        SimpleNurminenDetectionAlgorithm detector = new();
        BasicExtractionAlgorithm algorithm = new();

        IReadOnlyList<TableRectangle> regions = detector.Detect(pageArea);

        if (regions.Count == 0)
        {
            // Fallback to whole page if no tables were detected.
            // This is needed for some documents where the table detection fails, but the extraction still works.
            
            ExtractionApproach = TableExtractionApproach.WholePage;

            IEnumerable<P2PdfTable> detectedOnWholePage = algorithm.Extract(pageArea)
                .Select(x => new P2PdfTable(x));

            foreach (P2PdfTable p2PdfTable in detectedOnWholePage)
                yield return p2PdfTable;
        }
        else
        {
            ExtractionApproach = TableExtractionApproach.NurminenAlgorithm;
            
            foreach (TableRectangle region in regions)
            {
                PageArea tableArea = pageArea.GetArea(region.BoundingBox);

                IEnumerable<P2PdfTable> p2PdfTables = algorithm.Extract(tableArea)
                    .Select(x => new P2PdfTable(x));

                foreach (P2PdfTable p2PdfTable in p2PdfTables)
                    yield return p2PdfTable;
            }
        }
    }
}