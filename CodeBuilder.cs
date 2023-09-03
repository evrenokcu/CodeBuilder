using System.Text;

namespace CodeBuilder;

public abstract class CodeBuilder<TInterface, TClass> : Builder<TInterface, TClass>
    where TClass : TInterface where TInterface : class, ICodeBuilder
{
    protected readonly List<ICodeBuilder> SubBuilders = new();


    public string BuildLine(int indent) => Build(new StringBuilder(), indent).ToString();

    public string Name { get; }
    public ICodeBuilderBase Parent { get; }

    protected CodeBuilder(ICodeBuilderBase parent, string name)
    {
        Parent = parent;
        Name = name;
    }

    protected abstract void DoBuild(StringBuilder sb, int indent);

    public StringBuilder Build(StringBuilder sb, int indent = 0)
    {
        return BuildTemplate(sb, indent);
    }

    protected virtual StringBuilder BuildTemplate(StringBuilder sb, int indent = 0)
    {
        DoBuild(sb, indent);
        SubBuilders.ForEach(it => it.Build(sb, indent + 1));
        AfterSubBuild(sb, indent);
        return sb;
    }

    protected virtual void AfterSubBuild(StringBuilder sb, int indent)
    {
    }
}