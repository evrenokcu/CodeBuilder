namespace CodeBuilder.Implementations.IfStatementBuilder;

public class ComparisonConditionBuilder : CodeLineBuilder<IComparisonConditionInterface, ComparisonConditionBuilder>,
    IComparisonConditionInterface, IConditionBuilderBase
{
    public ComparisonConditionBuilder(ICodeBuilderBase parent, ComparisonOperation operation) : base(parent,
        "ComparisonCondition")
    {
        Operation = operation;
    }

    public ComparisonConditionBuilder(ICodeBuilderBase parent, ComparisonOperation operation,
        SimpleConditionBuilder left, SimpleConditionBuilder right) : base(parent, "ComparisonCondition")
    {
        Left = left;
        Right = right;
        Operation = operation;
    }

    public ComparisonConditionBuilder(ICodeBuilderBase parent, ComparisonOperation operation, string left, string right)
        : this(parent, operation)
    {
        Left = new SimpleConditionBuilder(this, left);
        Right = new SimpleConditionBuilder(this, right);
    }

    private ComparisonOperation Operation { get; set; }
    private IConditionBuilderBase Left { get; set; }
    private IConditionBuilderBase Right { get; set; }

    public override string BuildLine(int indent)
    {
        return $"{Left.BuildLine(indent)} {GetOperationString(Operation)} {Right.BuildLine(indent)}";
    }

    protected override ComparisonConditionBuilder GetBuilder()
    {
        return this;
    }

    private string GetOperationString(ComparisonOperation operation)
        => operation switch
        {
            ComparisonOperation.Equals => "==",
            ComparisonOperation.NotEquals => "!=",
            ComparisonOperation.GreaterThan => ">",
            _ => throw new InvalidOperationException("Not supported Comparison Operation")
        };

    public enum ComparisonOperation
    {
        Equals,
        NotEquals,
        GreaterThan
    }
}