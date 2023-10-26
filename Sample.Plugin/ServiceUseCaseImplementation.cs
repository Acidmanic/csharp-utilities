using Sample.Contract;

namespace Sample.Plugin;
public class ServiceUseCaseImplementation:IService
{
    public Model ProvideVeryImportantData()
    {
        return new Model
        {
            Very = "This is",
            Important = "Very Important",
            Data = "Provided use-case data"
        };
    }
}
