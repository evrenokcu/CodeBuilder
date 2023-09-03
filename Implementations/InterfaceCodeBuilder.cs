using System.Text;

namespace CodeBuilder.Implementations;

public class InterfaceCodeBuilder : CodeBuilder<IInterfaceBuilder, InterfaceCodeBuilder>, IInterfaceBuilder
{
    private List<string> BaseTypes { get; } = new();

    public InterfaceCodeBuilder(string name) : base(NullBuilder.Instance, name)
    {
    }

    public InterfaceCodeBuilder AddBaseType(string type)
    {
        BaseTypes.Add(type);
        return this;
    }

    protected override void DoBuild(StringBuilder sb, int indent)
    {
        sb.AppendLine(ClassBuilder.CreateInterfaceHeader(Name, BaseTypes.JoinStrings(", ")), indent)
            .AppendLine("{", indent);
    }

    public InterfaceFunctionCodeBuilder AddFunction(string name)
    {
        var builder = new InterfaceFunctionCodeBuilder(this, name);
        SubBuilders.Add(builder);
        return builder;
    }

    protected override void AfterSubBuild(StringBuilder sb, int indent)
    {
        sb.AppendLine("}", indent);
    }

    protected override InterfaceCodeBuilder GetBuilder()
    {
        return this;
    }
}