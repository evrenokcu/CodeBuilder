using System.Text;

namespace CodeBuilder;

public static class StringBuilderClassExtensions
{
    public static StringBuilder AppendComment(this StringBuilder sb, string comment)
    {
        return sb.AppendLine("//" + comment);
    }

    private static StringBuilder AppendLines(this StringBuilder sb, IEnumerable<string>? lines, int indent = 0,
        string prefix = "")
    {
        var enumerable = lines?.ToList();
        if (enumerable == null || !enumerable.Any())
            return sb;
        enumerable.ToList().ForEach(line => sb.AppendLine(prefix + line, indent));
        return sb;
    }

    public static StringBuilder AppendLine(this StringBuilder sb, string str, int indent) =>
        sb.AppendLine(Indents.Indent(indent) + str);


    public static StringBuilder AppendLines(this StringBuilder sb, params string[] lines)
    {
        return lines is { Length: > 0 } ? sb.AppendLine(lines.JoinStrings("\n")) : sb;
    }

    public static StringBuilder Append(this StringBuilder sb, string str, int indent) =>
        sb.Append(Indents.Indent(indent) + str);


    public static StringBuilder BuildLines(this StringBuilder sb, IList<ICodeLineBuilder> lines, int indent)
        => sb.AppendLines(lines.Select(line => line.BuildLine(indent)), indent);

    public static StringBuilder BuildCodes(this StringBuilder sb, IList<ICodeBuilder> codes, int indent)
    {
        codes.ToList().ForEach(code => code.Build(sb, indent));
        return sb;
    }
}