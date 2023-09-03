namespace CodeBuilder;

public interface ICodeBuilderBase
{
    string Name { get; }
    ICodeBuilderBase Parent { get; }
    string BuildLine(int indent);
}