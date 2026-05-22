using Tabula;
using UglyToad.PdfPig;

namespace DustInTheWind.NN.Toolkit.MandatoryPrivatePension.Pdf;

internal sealed class P2PdfDocument : IDisposable
{
    private readonly string filePath;
    private PdfDocument pdfDocument;
    private bool isDisposed;
    
    public DocumentParsingDiagnostics ParsingDiagnostics { get; private set; }

    public P2PdfDocument(string filePath)
    {
        this.filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
    }

    public void Open()
    {
        ThrowIfDisposed();

        if (pdfDocument is not null)
            return;

        ParsingOptions options = new()
        {
            ClipPaths = true
        };

        pdfDocument = PdfDocument.Open(filePath, options);
    }

    public IEnumerable<P2PdfTableRow> EnumerateRows()
    {
        ThrowIfDisposed();

        if (pdfDocument is null)
            throw new InvalidOperationException("The pdf document is not open. Call Open() before EnumerateRows().");

        ParsingDiagnostics = new DocumentParsingDiagnostics();
        IEnumerable<P2PdfPage> pages = EnumeratePages();

        foreach (P2PdfPage page in pages)
        {
            PageParsingDiagnostics pageParsingDiagnostics = ParsingDiagnostics.AddPage(page.PageIndex);
            IEnumerable<P2PdfTable> tables = page.EnumerateTables();

            foreach (P2PdfTable table in tables)
            {
                TableParsingDiagnostics tableParsingDiagnostics = pageParsingDiagnostics.AddTable();
                IEnumerable<P2PdfTableRow> rows = table.EnumerateRows();

                foreach (P2PdfTableRow row in rows)
                {
                    tableParsingDiagnostics.RowCount++;
                    yield return row;
                }
            }

            pageParsingDiagnostics.UsedFallbackExtraction = page.UsedFallbackExtraction;
        }
    }

    private IEnumerable<P2PdfPage> EnumeratePages()
    {
        for (int pageIndex = 1; pageIndex <= pdfDocument.NumberOfPages; pageIndex++)
        {
            PageArea pageArea = ObjectExtractor.Extract(pdfDocument, pageIndex);
            yield return new P2PdfPage(pageIndex, pageArea);
        }
    }

    private void ThrowIfDisposed()
    {
        if (isDisposed)
            throw new ObjectDisposedException(nameof(P2PdfDocument));
    }

    public void Dispose()
    {
        if (isDisposed)
            return;

        pdfDocument?.Dispose();
        pdfDocument = null;
        isDisposed = true;
    }
}