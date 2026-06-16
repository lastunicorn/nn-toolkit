using System.Globalization;
using DustInTheWind.NN.Toolkit.MandatoryPrivatePension.Pdf;

namespace DustInTheWind.NN.Toolkit.MandatoryPrivatePension;

internal static class P2PdfTableRowExtensions
{
	public static MonthDate GetMonthDate(this P2PdfTableRow row, int index)
	{
		if (index < 0 || index >= row.Count())
			throw new ArgumentOutOfRangeException(nameof(index));

		string rawValue = row[index];

		try
		{
			return MonthDate.Parse(rawValue);
		}
		catch (Exception ex)
		{
			throw new InvalidDataException(rawValue, row, ex);
		}
	}

	public static decimal GetDecimal(this P2PdfTableRow row, int index, CultureInfo cultureInfo)
	{
		if (index < 0 || index >= row.Count())
			throw new ArgumentOutOfRangeException(nameof(index));

		string rawValue = row[index];

		try
		{
			return decimal.Parse(rawValue, cultureInfo);
		}
		catch (Exception ex)
		{
			throw new InvalidDataException(rawValue, row, ex);
		}
	}
}