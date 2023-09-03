namespace CodeBuilder.Implementations;

public class FieldCodeBuilder : CodeLineBuilder<IFieldBuilder, FieldCodeBuilder>, IFieldBuilder
{
    private string Type { get; }
    private string Value { get; }

    public FieldCodeBuilder(ICodeBuilderBase parent, string name, string type, string value) : base(parent, name)
    {
        Type = type;
        Value = value;
    }

    public override string BuildLine(int indent)
    {
        return ClassBuilder.CreateField(Name, Type, Value);
    }

    protected override FieldCodeBuilder GetBuilder()
    {
        return this;
    }
}