using DustInTheWind.NN.Toolkit.MandatoryPrivatePension.Pdf;

namespace DustInTheWind.NN.Toolkit.MandatoryPrivatePension;

public class DocumentLoadResult
{
    public ContributionsDocument Document { get; set; }

    public DocumentParsingDiagnostics Diagnostics { get; set; }
}