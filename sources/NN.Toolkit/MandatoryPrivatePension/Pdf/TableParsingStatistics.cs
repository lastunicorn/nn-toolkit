namespace DustInTheWind.NN.Toolkit.MandatoryPrivatePension.Pdf;

public class TableParsingStatistics
{
    public int RowCount { get; set; }

    public Exception Error { get; set; }

    public bool HasErrors => Error != null;
}