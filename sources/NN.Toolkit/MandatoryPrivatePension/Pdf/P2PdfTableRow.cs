using System.Collections;
using Tabula;

namespace DustInTheWind.NN.Toolkit.MandatoryPrivatePension.Pdf;

internal class P2PdfTableRow : IEnumerable<string>
{
    private readonly string[] values = new string[7];

    public string this[int i] => values[i];

    public P2PdfTableRow(IReadOnlyList<RectangularTextContainer> row)
    {
        if (row.Count >= 15)
            ParseWideRow(row);
        else
            ParseNarrowRow(row);
    }

    private void ParseWideRow(IReadOnlyList<RectangularTextContainer> row)
    {
        values[0] = row[0].GetText().Trim();
        values[1] = row[1].GetText().Trim();
        values[2] = row[3].GetText().Trim();
        values[3] = row[6].GetText().Trim();
        values[4] = row[8].GetText().Trim();
        values[5] = row[11].GetText().Trim();
        values[6] = row[14].GetText().Trim();
    }

    private void ParseNarrowRow(IReadOnlyList<RectangularTextContainer> row)
    {
        values[0] = row[0].GetText().Trim();
        values[1] = row[1].GetText().Trim();
        values[2] = row[2].GetText().Trim();
        values[3] = row[3].GetText().Trim();
        values[4] = row[4].GetText().Trim();
        values[5] = row[5].GetText().Trim();
        values[6] = row[6].GetText().Trim();
    }

    public IEnumerator<string> GetEnumerator()
    {
        return ((IEnumerable<string>)values).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}