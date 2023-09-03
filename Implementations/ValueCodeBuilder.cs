namespace CodeBuilder.Implementations;

public class ValueCodeBuilder : CodeLineBuilder<IValueCodeBuilder, ValueCodeBuilder>, IValueCodeBuilder
{
    public ValueCodeBuilder(ICodeBuilderBase parent, string value) : base(parent, value) { }
    public override string BuildLine(int indent) => Name;

    protected override ValueCodeBuilder GetBuilder()
    {
        return this;
    }
}