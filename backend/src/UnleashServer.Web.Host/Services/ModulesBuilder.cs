namespace UnleashServer.Web.Host.Services;

internal class ModulesBuilder
{
    private readonly IList<Type> _moduleTypes = new List<Type>();

    public ModulesBuilder Add<T>()
    {
        var moduleType = typeof(T);
        if(!_moduleTypes.Contains(moduleType))
            _moduleTypes.Add(moduleType);
        return this;
    }

    public Type[] Build() => _moduleTypes.ToArray();

    public static ModulesBuilder Create() => new ModulesBuilder();
}