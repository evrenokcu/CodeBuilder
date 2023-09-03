namespace CodeBuilder;

public static class Indents
{
    private static readonly string[] Indentations = { "", "\t", "\t\t", "\t\t\t", "\t\t\t\t", "\t\t\t\t\t" };
    public static string Indent(int i) => Indentations[i];
}