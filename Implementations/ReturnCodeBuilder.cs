namespace CodeBuilder.Implementations;

public class ReturnCodeBuilder : CodeLineBuilder<IReturnBuilder, ReturnCodeBuilder>, IReturnBuilder
{
    private ICodeBuilderBase Value { get; set; }
    public ReturnCodeBuilder(ICodeBuilderBase parent, string variableName)
        : base(parent, variableName)
    {
        if (!string.IsNullOrWhiteSpace(variableName))
            Value = new ValueCodeBuilder(this, Convert.ToString(variableName));

    }

    public override string BuildLine(int indent)
    {
        return ClassBuilder.CreateReturn(Value.BuildLine(indent));
    }
    public T ReturnWith<T>(Func<ICodeBuilderBase, T> factory) where T : ICodeBuilderBase
    {
        var valueCodeBuilder = factory.Invoke(this);
        Value = valueCodeBuilder;
        return valueCodeBuilder;
    }

    protected override ReturnCodeBuilder GetBuilder()
    {
        return this;
    }
}