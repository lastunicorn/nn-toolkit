namespace DustInTheWind.NN.Toolkit.Cli.Ports.FileSystemAccess;

public class FileSystemService : IFileSystemService
{
    public string ReadAllText(string filePath)
    {
        if (filePath == null) throw new ArgumentNullException(nameof(filePath));

        return File.ReadAllText(filePath);
    }

    public IEnumerable<string> ReadTextLines(string filePath)
    {
        if (filePath == null) throw new ArgumentNullException(nameof(filePath));

        StreamReader reader = new(filePath);

        while (!reader.EndOfStream)
        {
            yield return reader.ReadLine();
        }
    }

    public StreamReader OpenStreamReader(string filePath)
    {
        if (filePath == null) throw new ArgumentNullException(nameof(filePath));

        return new StreamReader(filePath);
    }
}