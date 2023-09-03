using System.Text;

namespace CodeBuilder.Implementations;

public class EnumCodeBuilder : CodeBuilder<IEnumBuilder, EnumCodeBuilder>, IEnumBuilder
{
    private List<string> EnumValues { get; } = new();

    public EnumCodeBuilder(string name) : base(NullBuilder.Instance, name)
    {
    }

    public EnumCodeBuilder AddValues(IEnumerable<string> values)
    {
        EnumValues.AddRange(values);
        return this;
    }

    protected override void DoBuild(StringBuilder sb, int indent)
    {
        sb.AppendLine(ClassBuilder.CreateEnumHeader(Name), indent);
        sb.AppendLine("{");
        sb.AppendLine(EnumValues.JoinStrings(", "), indent + 1);
        sb.AppendLine("}");
    }

    protected override EnumCodeBuilder GetBuilder()
    {
        return this;
    }
}