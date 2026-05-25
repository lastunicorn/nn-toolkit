using System.Collections.ObjectModel;
using System.Globalization;
using DustInTheWind.NN.Toolkit.MandatoryPrivatePension.Pdf;

namespace DustInTheWind.NN.Toolkit.MandatoryPrivatePension;

public class ContributionsDocument : Collection<Contribution>
{
	public ContributionsHeader Header { get; } = [];

	public static DocumentLoadResult LoadFromFile(string filePath, CultureInfo cultureInfo = null)
	{
		cultureInfo ??= CultureInfo.InvariantCulture;

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
				GrossValue = row.GetDecimal(1, cultureInfo),
				AdministrationFee = row.GetDecimal(2, cultureInfo),
				NetValue = row.GetDecimal(3, cultureInfo),
				UnitValue = row.GetDecimal(4, cultureInfo),
				UnitCount = row.GetDecimal(5, cultureInfo),
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