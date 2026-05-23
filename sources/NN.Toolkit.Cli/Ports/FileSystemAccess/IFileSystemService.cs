namespace DustInTheWind.NN.Toolkit.Cli.Ports.FileSystemAccess;

public interface IFileSystemService
{
    string ReadAllText(string filePath);
    IEnumerable<string> ReadTextLines(string filePath);
    StreamReader OpenStreamReader(string filePath);
}