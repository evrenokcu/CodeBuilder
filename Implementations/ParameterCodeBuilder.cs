namespace CodeBuilder.Implementations;

public class ParameterCodeBuilder : CodeLineBuilder<IParameterBuilder, ParameterCodeBuilder>, IParameterBuilder
{
    private string _value = string.Empty;
    private string ParameterType { get; set; }
    public ParameterCodeBuilder(ICodeBuilderBase parent, string name, string parameterType) : base(parent, name)
    {
        ParameterType = parameterType;
    }

    public override string BuildLine(int indent) => $"{ParameterType} {Name}{_value.IfDefined(() => $" = {_value}")}";

    public ParameterCodeBuilder Value(string value)
    {
        _value = value;
        return this;
    }

    protected override ParameterCodeBuilder GetBuilder()
    {
        return this;
    }
}