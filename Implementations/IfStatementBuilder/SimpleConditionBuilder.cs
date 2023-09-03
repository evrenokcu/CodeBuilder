namespace CodeBuilder.Implementations.IfStatementBuilder;

public class SimpleConditionBuilder : CodeLineBuilder<ISimpleConditionBuilder, SimpleConditionBuilder>,
    ISimpleConditionBuilder, IConditionBuilderBase
{
    private bool _isNegate;
    private ICodeBuilderBase Value { get; set; }

    public SimpleConditionBuilder(ICodeBuilderBase parent, string condition) : base(parent, "SimpleCondition")
    {
        Condition = condition;
    }

    public string Condition { get; set; }


    protected override SimpleConditionBuilder GetBuilder()
    {
        return this;
    }
    public override string BuildLine(int indent)
    {
        return ClassBuilder.CreateCondition(Value?.BuildLine(indent) ?? Condition, _isNegate);
    }

    public SimpleConditionBuilder Negate()
    {
        _isNegate = true;
        return this;
    }

    public SimpleConditionBuilder ConditionWith<T>(Func<SimpleConditionBuilder, T> factory) where T : ICodeBuilderBase
    {
        var valueCodeBuilder = factory.Invoke(this);
        Value = valueCodeBuilder;
        return this;
    }
}