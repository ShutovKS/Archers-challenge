namespace Infrastructure.Services.SceneDependency
{
    public interface ISceneDependencyProvider
    {
        T GetDependency<T>();
    }
}