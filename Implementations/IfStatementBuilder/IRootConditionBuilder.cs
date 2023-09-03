namespace CodeBuilder.Implementations.IfStatementBuilder;

public interface IRootConditionBuilder
{
    ConditionGroupBuilder And();
    ConditionGroupBuilder Or();
    ComparisonConditionBuilder Equals();
    ComparisonConditionBuilder NotEquals();
    ComparisonConditionBuilder Equals(string left, string right);
    ComparisonConditionBuilder NotEquals(string left, string right);
    SimpleConditionBuilder IfTrue(string condition);
}