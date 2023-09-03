using System.Text;

namespace CodeBuilder;

public class NullBuilder : ICodeBuilder
{
    public static readonly NullBuilder Instance = new();
    public ICodeBuilderBase Parent => this;

    public string BuildLine(int indent) => string.Empty;

    public string Name => string.Empty;

    public StringBuilder Build(StringBuilder sb, int indent)
    {
        return sb;
    }
}