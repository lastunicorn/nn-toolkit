using Tabula;

namespace DustInTheWind.NN.Toolkit.MandatoryPrivatePension.Pdf;

internal class P2PdfTable
{
	private readonly Table table;

	public P2PdfTable(Table table)
	{
		this.table = table ?? throw new ArgumentNullException(nameof(table));
	}

	public IEnumerable<P2PdfTableRow> EnumerateRows()
	{
		return table.Rows
			.Select(x => new P2PdfTableRow(x));
	}
}