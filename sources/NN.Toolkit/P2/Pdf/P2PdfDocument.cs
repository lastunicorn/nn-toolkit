using Tabula;
using UglyToad.PdfPig;

namespace DustInTheWind.NN.Toolkit.P2.Pdf;

internal class ParsingStatistics
{
    public int PageCount { get; set; }

    public int TableCount { get; set; }
    
    public Exception Error { get; set; }
}

internal class P2PdfDocument
{
    private readonly string filePath;
    private ParsingStatistics parsingStatistics;

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

        parsingStatistics = new ParsingStatistics();
        using PdfDocument pdfDocument = PdfDocument.Open(filePath, options);
        
        IEnumerable<P2PdfPage> pages = EnumeratePages(pdfDocument);

        foreach (P2PdfPage page in pages)
        {
            parsingStatistics.PageCount++;
            IEnumerable<P2PdfTable> tables = page.EnumerateTables();

            foreach (P2PdfTable table in tables)
            {
                parsingStatistics.TableCount++;
                IEnumerable<P2PdfTableRow> rows = table.EnumerateRows();
                
                foreach (P2PdfTableRow row in rows)
                    yield return row;
            }
        }
    }

    private IEnumerable<P2PdfPage> EnumeratePages(PdfDocument pdfDocument)
    {
        for (int pageIndex = 1; pageIndex <= pdfDocument.NumberOfPages; pageIndex++)
        {
            PageArea pageArea = ObjectExtractor.Extract(pdfDocument, pageIndex);
            yield return new P2PdfPage(pageArea);
        }
    }
}