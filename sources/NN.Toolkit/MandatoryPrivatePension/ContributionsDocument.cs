using System.Collections.ObjectModel;
using DustInTheWind.NN.Toolkit.MandatoryPrivatePension.Pdf;

namespace DustInTheWind.NN.Toolkit.MandatoryPrivatePension;

public class ContributionsDocument : Collection<Contribution>
{
    public ContributionsHeader Header { get; } = [];

    public static DocumentLoadResult LoadFromFile(string filePath)
    {
        ContributionsDocument document = [];

        using P2PdfDocument pdfDocument = new(filePath);
        pdfDocument.Open();
        IEnumerable<P2PdfTableRow> rows = pdfDocument.EnumerateRows();

        using IEnumerator<P2PdfTableRow> rowEnumerator = rows.GetEnumerator();

        if (rowEnumerator.MoveNext())
            document.Header.AddRange(rowEnumerator.Current);

        while (rowEnumerator.MoveNext())
        {
            P2PdfTableRow row = rowEnumerator.Current;

            document.Add(new Contribution
            {
                Month = row.GetMonthDate(0),
                GrossValue = row.GetDecimal(1),
                AdministrationFee = row.GetDecimal(2),
                NetValue = row.GetDecimal(3),
                UnitValue = row.GetDecimal(4),
                UnitCount = row.GetDecimal(5),
                PaidInMonth = row.GetMonthDate(6)
            });
        }

        return new DocumentLoadResult
        {
            Document = document,
            Diagnostics = pdfDocument.ParsingDiagnostics
        };
    }
}