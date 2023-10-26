namespace Sample.Contract;

public interface IService
{

    public string Name { get; }
    public Model ProvideVeryImportantData();
}