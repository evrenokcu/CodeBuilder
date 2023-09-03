namespace CodeBuilder;

public abstract class CodeLineBuilder<TInterface, TClass> : Builder<TInterface, TClass>
    where TClass : TInterface where TInterface : class
{
    protected CodeLineBuilder(ICodeBuilderBase parent, string name)
    {
        Name = name;
        Parent = parent;
    }

    public ICodeBuilderBase Parent { get; }
    public string Name { get; }

    public abstract string BuildLine(int indent);
}