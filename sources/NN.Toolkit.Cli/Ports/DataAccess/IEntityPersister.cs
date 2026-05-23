namespace DustInTheWind.NN.Toolkit.Cli.DataAccess;

public interface IEntityPersister<T>
{
    IEnumerable<T> Load();
    
    void Save(IEnumerable<T> entities);
}