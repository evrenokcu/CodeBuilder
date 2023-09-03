namespace CodeBuilder.Implementations;

public class ArgumentNullCheckCodeBuilder : CodeLineBuilder<IArgumentNullCheckCodeBuilder, ArgumentNullCheckCodeBuilder>, IArgumentNullCheckCodeBuilder
{
    public ArgumentNullCheckCodeBuilder(ICodeBuilderBase parent, string name) : base(parent, name)
    {
    }

    public override string BuildLine(int indent) =>
        ClassBuilder.CreateArgumentNullCheck(Name);

    protected override ArgumentNullCheckCodeBuilder GetBuilder()
    {
        return this;
    }
}