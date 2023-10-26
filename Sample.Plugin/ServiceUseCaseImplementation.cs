using Sample.Contract;
using Sample.Dependency;

namespace Sample.Plugin;
public class ServiceUseCaseImplementation:IService
{
    public string Name => "UseCase-1";

    public Model ProvideVeryImportantData()
    {
        return new Model
        {
            Very = "This is",
            Important = "Very Important",
            Data = new TextUpperCaseService().ToUpper("Provided use-case data")
        };
    }
}
