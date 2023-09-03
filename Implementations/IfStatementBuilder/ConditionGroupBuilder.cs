namespace CodeBuilder.Implementations.IfStatementBuilder;

public class ConditionGroupBuilder : CodeLineBuilder<IConditionGroupBuilder, ConditionGroupBuilder>,
    IConditionGroupBuilder, IConditionBuilderBase
{
    public ConditionGroupBuilder(ICodeBuilderBase parent, ConditionGroupOperation operation) : base(parent,
        "ConditionGroup")
    {
        Operation = operation;
    }

    private ConditionGroupOperation Operation { get; set; }
    private List<IConditionBuilderBase> Conditions { get; set; } = new List<IConditionBuilderBase>();

    public SimpleConditionBuilder AddCondition(string condition)
    {
        var builder = new SimpleConditionBuilder(this, condition);
        Conditions.Add(builder);
        return builder;
    }


    public ComparisonConditionBuilder Equals(string left, string right)
    {
        var builder = new ComparisonConditionBuilder(this, ComparisonConditionBuilder.ComparisonOperation.Equals, left, right);
        Conditions.Add(builder);
        return builder;
    }


    public ComparisonConditionBuilder NotEquals(string left, string right)
    {
        var builder = new ComparisonConditionBuilder(this, ComparisonConditionBuilder.ComparisonOperation.NotEquals, left, right);
        Conditions.Add(builder);
        return builder;
    }

    private string GetOperationString(ConditionGroupOperation operation)
        => operation switch
        {
            ConditionGroupOperation.And => "&&",
            ConditionGroupOperation.Or => "||",
            _ => throw new InvalidOperationException("ConditionGroup operation is not supported.")
        };


    public enum ConditionGroupOperation
    {
        And,
        Or
    }

    public ConditionGroupBuilder And()
    {
        var builder = new ConditionGroupBuilder(this, ConditionGroupOperation.And);
        Conditions.Add(builder);
        return builder;
    }

    public ConditionGroupBuilder Or()
    {
        var builder = new ConditionGroupBuilder(this, ConditionGroupOperation.Or);
        Conditions.Add(builder);
        return builder;
    }

    protected override ConditionGroupBuilder GetBuilder()
    {
        return this;
    }

    public override string BuildLine(int indent)
    {
        return
            $"({string.Join(GetOperationString(Operation), Conditions.Select(condition => condition.BuildLine(indent)))})";
    }
}