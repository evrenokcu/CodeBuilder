namespace CodeBuilder.Implementations.IfStatementBuilder;

public class RootCondition : CodeLineBuilder<IRootConditionBuilder, RootCondition>, IRootConditionBuilder,
    IConditionBuilderBase
{
    private IConditionBuilderBase Condition { get; set; }

    public RootCondition(ICodeBuilderBase parent) : base(parent, "rootCondition")
    {
    }

    public override string BuildLine(int indent) => $"({Condition.BuildLine(indent)})";


    public ConditionGroupBuilder And()
    {
        if (Condition != null) throw new InvalidOperationException("Condition already defined.");
        var builder = new ConditionGroupBuilder(this, ConditionGroupBuilder.ConditionGroupOperation.And);
        Condition = builder;
        return builder;
    }

    public ConditionGroupBuilder Or()
    {
        if (Condition != null) throw new InvalidOperationException("Condition already defined.");

        var builder = new ConditionGroupBuilder(this, ConditionGroupBuilder.ConditionGroupOperation.Or);
        Condition = builder;
        return builder;
    }

    public ComparisonConditionBuilder Equals()
    {
        if (Condition != null) throw new InvalidOperationException("Condition already defined.");
        var builder = new ComparisonConditionBuilder(this, ComparisonConditionBuilder.ComparisonOperation.Equals);
        Condition = builder;
        return builder;
    }

    public ComparisonConditionBuilder EqualsToNull(string valueToCompare)
    {
        if (Condition != null) throw new InvalidOperationException("Condition already defined.");
        var builder = new ComparisonConditionBuilder(this, ComparisonConditionBuilder.ComparisonOperation.Equals,
            "null", valueToCompare);
        Condition = builder;
        return builder;
    }


    public ComparisonConditionBuilder NotEquals()
    {
        if (Condition != null) throw new InvalidOperationException("Condition already defined.");

        var builder = new ComparisonConditionBuilder(this, ComparisonConditionBuilder.ComparisonOperation.Equals);
        Condition = builder;
        return builder;
    }


    public ComparisonConditionBuilder Equals(string left, string right)
    {
        if (Condition != null) throw new InvalidOperationException("Condition already defined.");
        var builder = new ComparisonConditionBuilder(this, ComparisonConditionBuilder.ComparisonOperation.Equals,
            new SimpleConditionBuilder(this, left), new SimpleConditionBuilder(this, right));
        Condition = builder;
        return builder;
    }

    public ComparisonConditionBuilder NotEquals(string left, string right)
    {
        if (Condition != null) throw new InvalidOperationException("Condition already defined.");
        var builder = new ComparisonConditionBuilder(this, ComparisonConditionBuilder.ComparisonOperation.NotEquals,
            new SimpleConditionBuilder(this, left), new SimpleConditionBuilder(this, right));
        Condition = builder;
        return builder;
    }

    public SimpleConditionBuilder IfTrue(string condition)
    {
        if (Condition != null) throw new InvalidOperationException("Condition already defined.");
        var builder = new SimpleConditionBuilder(this, condition);
        Condition = builder;
        return builder;
    }

    public ComparisonConditionBuilder IfGreater(string left, string right)
    {
        if (Condition != null) throw new InvalidOperationException("Condition already defined.");
        var builder = new ComparisonConditionBuilder(this, ComparisonConditionBuilder.ComparisonOperation.NotEquals,
            new SimpleConditionBuilder(this, left), new SimpleConditionBuilder(this, right));
        Condition = builder;
        return builder;
    }

    public SimpleConditionBuilder IfNot(string condition)
    {
        if (Condition != null) throw new InvalidOperationException("Condition already defined.");
        var builder = new SimpleConditionBuilder(this, condition).Negate();
        Condition = builder;
        return builder;
    }

    protected override RootCondition GetBuilder()
    {
        return this;
    }
}