namespace CodeBuilder.Implementations.IfStatementBuilder;

public interface IConditionGroupBuilder
{
    SimpleConditionBuilder AddCondition(string condition);
    ComparisonConditionBuilder Equals(string left, string right);
    ComparisonConditionBuilder NotEquals(string left, string right);
    ConditionGroupBuilder And();
    ConditionGroupBuilder Or();
}