using System.Collections.ObjectModel;
using DustInTheWind.NN.Toolkit.P2.Pdf;

namespace DustInTheWind.NN.Toolkit.P2;

public class ContributionsDocument : Collection<Contribution>
{
    public ContributionsHeader Header { get; } = [];

    public static ContributionsDocument LoadFromFile(string filePath)
    {
        ContributionsDocument document = [];

        P2PdfDocument pdfDocument = new(filePath);
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

        return document;
    }
}