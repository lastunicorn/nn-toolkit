namespace DustInTheWind.NN.Toolkit.Cli.Ports.DataAccess;

public interface IEntityPersister<T>
{
    IEnumerable<T> Load();
    
    void Save(IEnumerable<T> entities);
}