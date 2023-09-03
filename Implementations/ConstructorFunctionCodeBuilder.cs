using System.Text;

namespace CodeBuilder.Implementations;

public class ConstructorFunctionCodeBuilder : FunctionCodeBuilder
{
    public ConstructorFunctionCodeBuilder(ICodeBuilder parent, string name) : base(parent, name)
    {
    }

    protected override void BuildHeader(StringBuilder sb, int indent) =>
        sb.AppendLine(
            ClassBuilder.CreateFunctionHeader(
                Name,
                Parameters.Select(it => it.BuildLine(indent)),
                AccessModifier,
                baseCall: BaseCall?.BuildLine(indent)!),
            indent);
}