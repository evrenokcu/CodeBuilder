namespace CodeBuilder.Implementations;

public class FunctionCallParameterCodeBuilder : CodeLineBuilder<IFunctionCall, FunctionCallParameterCodeBuilder>,
    IFunctionCall
{
    private ICodeBuilderBase Value { get; set; }

    public FunctionCallParameterCodeBuilder(ICodeBuilderBase parent, string name) : base(parent, name)
    {
    }

    public override string BuildLine(int indent)
    {
        return Value?.BuildLine(indent) ?? Name;
    }

    public T ParameterWith<T>(Func<ICodeBuilderBase, T> factory) where T : ICodeBuilderBase
    {
        var valueCodeBuilder = factory.Invoke(this);
        Value = valueCodeBuilder;
        return valueCodeBuilder;
    }

    protected override FunctionCallParameterCodeBuilder GetBuilder()
    {
        return this;
    }
}