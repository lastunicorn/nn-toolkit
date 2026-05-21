namespace DustInTheWind.NN.Toolkit.P2;

public class InvalidDataException : Exception
{
    public InvalidDataException(string value, IEnumerable<string> values)
        : base($"Data could not be parsed: {value}. The entire row: {string.Join(" | ", values)}")
    {
    }
    
    public InvalidDataException(string value, IEnumerable<string> values, Exception innerException)
        : base($"Data could not be parsed: {value}. The entire row: {string.Join(" | ", values)}",  innerException)
    {
    }
}